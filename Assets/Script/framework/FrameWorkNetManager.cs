using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using UnityEngine;


public enum NetEvent
{
    ConnectSucc = 1,
    ConnectFail = 2,
    Close = 3,
}

public class FrameWorkNetManager
{
    private static Socket socket;
    private static FrameWorkByteArray readBuff;
    private static Queue<FrameWorkByteArray> writeQueue;

    public delegate void EventListener(string err);
    private static Dictionary<NetEvent,EventListener> eventListeners = new Dictionary<NetEvent, EventListener>();
    private static bool isConnecting = false;
    private static bool isClosing = false;

    static List<FrameWorkMsgBase> msgList ;
    //消息列表长度
    private static int msgCount = 0;

    private readonly static int MAX_MESSAGE_FIRE = 10;


    //消息委托类型
    public delegate void MsgListener(FrameWorkMsgBase msgBase);
    private static Dictionary<string ,MsgListener> msgListeners = new Dictionary<string, MsgListener>();


    //心跳定义内容
    //是否启用心跳
    public static bool isUsePing = true;
    //心跳间隔时间
    public static int pingInterval = 30;
    //上一次发送 ping 的时间
    private static float lastPingTime = 0;
    //上一次收到 pong 的时间
    private static float lastPongTime = 0;


    public static void AddEventListener(NetEvent netEvent, EventListener listener)
    {
        if (eventListeners.ContainsKey(netEvent))
            eventListeners[netEvent] += listener;
        else
            eventListeners[netEvent] = listener;
    }

    public static void RemoveEventListener(NetEvent netEvent, EventListener listener)
    {
        if (eventListeners.ContainsKey(netEvent))
        {
            eventListeners[netEvent] -= listener;
            if (eventListeners[netEvent] == null)
                eventListeners.Remove(netEvent);
        }
    }


    private static void FireEvent(NetEvent netEvent, string str)
    {
        if (eventListeners.ContainsKey(netEvent))
        {
            eventListeners[netEvent](str);
        }
    }

    public static void Connect(string ip, int port)
    {
        if (socket != null && socket.Connected)
        {
            Debug.Log("Connect fail , already connected!");
            return;
        }

        if (isConnecting)
        {
            Debug.Log("Connect fail , isConnecting!");
            return;
        }
        InitState();
        socket.NoDelay = true;
        isConnecting = true;
        socket.BeginConnect(ip, port, ConnectCallBack, socket);
    }

    public static void ConnectCallBack(IAsyncResult ia)
    {
        try
        {
            Socket socket = (Socket)ia.AsyncState;
            socket.EndConnect(ia);
            Debug.Log("Socket Connect Successful");
            FireEvent(NetEvent.ConnectSucc,"");
            
            isConnecting = false;
            socket.BeginReceive(readBuff.bytes, readBuff.writeIndex, readBuff.remain, 0, ReceiveCallBack, socket);
        }
        catch (SocketException e)
        {
            Console.WriteLine("Socket Connect Fail"+e);

            FireEvent(NetEvent.ConnectFail, e.ToString());
            throw;
        }
    }

    static void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket) ar.AsyncState;
            //获取接受的信息长度
            int count = socket.EndReceive(ar);

            if (count == 0)
            {
                Close();
                return;
            }

            readBuff.writeIndex += count;
            OnReceiveDate();
            if (readBuff.remain<8)
            {
                readBuff.MoveBytes();
                readBuff.ReSize(readBuff.length*2);
            }
            socket.BeginReceive(readBuff.bytes, readBuff.writeIndex, readBuff.remain, 0, ReceiveCallBack, socket);
        }
        catch (SocketException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    static void OnReceiveDate()
    {
        if (readBuff.length<=2)
        {
            return;
        }
        //获取消息体长度
        int readIdx = readBuff.readIndex;
        byte[] bytes = readBuff.bytes;
        Int16 bodyLength = (Int16) (bytes[readIdx + 1] << 8 | bytes[readIdx]);
        if (readBuff.length<bodyLength+2)
        {
            return;
        }
        readBuff.readIndex += 2;

        //解析协议名
        int nameCount = 0;
        string protoName = FrameWorkMsgBase.DecodeName(readBuff.bytes, readBuff.readIndex, out nameCount);
        if (protoName == "")
        {
            Debug.Log("OnReceiveData MsgBase.DecodeName Fail");
            return;
        }

        readBuff.readIndex += nameCount;

        //解析协议体
        int bodyCount = bodyLength - nameCount;
        FrameWorkMsgBase msgBase = FrameWorkMsgBase.Decode(protoName,readBuff.bytes,readBuff.readIndex,bodyCount);

        readBuff.readIndex += bodyCount;
        readBuff.CheckAndMoveBytes();
        //添加消息队列
        lock (msgList)
        {
            msgList.Add(msgBase);
        }

        msgCount++;

        //继续读取消息
        if (readBuff.length>2)
        {
            OnReceiveDate();
        }
    }

    public static void Close()
    {
        if (socket==null || !socket.Connected)
        {
            return;
        }

        if (isConnecting)
        {
            return;
        }

        if (writeQueue.Count>0)
        {
            isClosing = true;
        }
        else
        {
            socket.Close();
            FireEvent(NetEvent.Close,"");
        }
    }

    //初始化socket状态
    public static void  InitState()
    {
        socket = new  Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
        readBuff = new FrameWorkByteArray();
        writeQueue = new Queue<FrameWorkByteArray>();
        isConnecting = false;
        isClosing = false;

        msgList = new List<FrameWorkMsgBase>();
        msgCount = 0;
        //上一次发送 ping 的时间
        lastPingTime = Time.time;
        //上一次收到 pong 的时间
        lastPongTime = Time.time;

        //监听 pong 协议
        if (!msgListeners.ContainsKey("MsgPong"))
        {
            AddMsgListener("MsgPong", OnMsgPong);
        }
    }

    public static void OnMsgPong(FrameWorkMsgBase msgBase)
    {
        lastPongTime = Time.time;
    }

    //发送协议
    public static void Send(FrameWorkMsgBase msg)
    {
        if (socket == null || !socket.Connected)
        {
            return;
        }

        if (isConnecting)
        {
            return;
        }

        if (isClosing)
        {
            return;
        }

        //数据编码
        byte[] nameBytes = FrameWorkMsgBase.EncodeName(msg);

        byte[] bodyBytes = FrameWorkMsgBase.Encode(msg);
        int len = nameBytes.Length + bodyBytes.Length;
        byte [] sendBytes = new byte[2+len];

        //组装长度
        sendBytes[0] = (byte) (len % 256);
        sendBytes[1] = (byte) (len / 256);

        //组装名字
        Array.Copy(nameBytes,0,sendBytes,2,nameBytes.Length);

        //组装消息体
        Array.Copy(bodyBytes,0,sendBytes,2+nameBytes.Length,bodyBytes.Length);

        FrameWorkByteArray ba = new FrameWorkByteArray(sendBytes);
        int count = 0;
        lock (writeQueue)
        {
            writeQueue.Enqueue(ba);
            count = writeQueue.Count;
        }

        //发送消息
        if (count ==1)
        {
            socket.BeginSend(sendBytes, 0, sendBytes.Length, 0, SendCallBack, socket);
        }
    }

    //发送消息回调
    public static void SendCallBack(IAsyncResult ar)
    {
        Socket socket = (Socket) ar.AsyncState;
        if (socket == null|| !socket.Connected)
        {
            return;
        }
        //发送结束
        int count = socket.EndSend(ar);
        //获取写入队列第一条数据
        FrameWorkByteArray ba = new FrameWorkByteArray();
        lock (ba)
        {
            ba = writeQueue.First();
        }

        //完整发送
        ba.readIndex += count;
        if (ba.length == 0)
        {
            lock (writeQueue)
            {
                writeQueue.Dequeue();
                ba = writeQueue.First();
            }
        }

        if (ba != null)
        {
            socket.BeginSend(ba.bytes, ba.readIndex, ba.length, 0, SendCallBack, socket);
        }
        else if(isClosing)
        {
            socket.Close();
        }
    }

    public static void Update()
    {
        MsgUpdate();
        //心跳包
        PingUpdate();
    }

    public static void MsgUpdate()
    {
        //初步判断，提升效率
        if (msgCount==0)
        {
            return;
        }

        //重复处理消息
        for (int i = 0; i < MAX_MESSAGE_FIRE; i++)
        {
            FrameWorkMsgBase msgBase = null;
            //获取第一条消息
            lock (msgList)
            {
                if (msgList.Count>0)
                {
                    msgBase = msgList[0];
                    msgList.RemoveAt(0);
                    msgCount--;
                }
            }

            if (msgBase!=null)
            {
                FireMsg(msgBase.protoName,msgBase);
            }
            else
            {
                break;
            }
        }
    }


    public static void PingUpdate()
    {
        //是否启用
        if (!isUsePing)
        {
            return;
        }
        //发送ping
        if (Time.time - lastPingTime >pingInterval)
        {
            MsgPing msgPing = new MsgPing();
            Send(msgPing);
            lastPingTime = Time.time;
        }

        //检查Pong时间
        if (Time.time - lastPongTime > pingInterval*4)
        {
            Close();
        }
    }


    //消息处理逻辑

    public static void AddMsgListener(string msgName, MsgListener listener)
    {
        if (msgListeners.ContainsKey(msgName))
        {
            msgListeners[msgName] += listener;
        }
        else
        {
            msgListeners[msgName] = listener;
        }
    }

    public static void RemoveMsgListenr(string msgName, MsgListener listener)
    {
        if (msgListeners.ContainsKey(msgName))
        {
            msgListeners[msgName] -= listener;
            if (msgListeners[msgName] == null)
            {
                msgListeners.Remove(msgName);
            }
        }
    }

    private static void FireMsg(string msgName, FrameWorkMsgBase msgBase)
    {
        if (msgListeners.ContainsKey(msgName))
        {
            msgListeners[msgName](msgBase);
        }
    }
}
