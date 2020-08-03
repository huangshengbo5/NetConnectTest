using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class MoveNetManager  {

    public static MoveNetManager instance;
    GameObject NetObj;

    public static MoveNetManager GetInstance()
    {
        if (instance == null)
        {
            instance = new MoveNetManager();
        }
        return instance;
    }

    byte [] receiveMessage = new byte[1024];

    Socket socket;
    public void StartConnect()
    {
        socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
        //socket.Connect("127.0.0.1", 9999);
        socket.BeginConnect("127.0.0.1", 9999, ConnectCallBack, socket);
    }

    public void ConnectCallBack(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndConnect(ar);
            socket.BeginReceive(receiveMessage, 0, 1024, 0,ReceiveCallBack, socket);
        }
        catch (SocketException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    //收到服务器发来的信息
    public void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket) ar.AsyncState;
            int count =  socket.EndReceive(ar);
            string message = System.Text.Encoding.Default.GetString(receiveMessage, 0, count);
            UnPack(message);
            socket.BeginReceive(receiveMessage, 0, 1024, 0, ReceiveCallBack, socket);
        }
        catch (SocketException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    public void SendNetMessage(string message)
    {
        byte[] messageByte = System.Text.Encoding.Default.GetBytes(message);
        socket.BeginSend(messageByte,0,messageByte.Length,0, SendCallBack, socket);

    }

    public void SendCallBack(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndSend(ar);
        }
        catch (SocketException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    public void Pack(string message)
    {

    }

    //解包
    //strMessages
    //[0]:消息类型
    //[1]:消息内容

    public void UnPack(string message)
    {
        string[] strMessages = message.Split(',');
        int type = int.Parse(strMessages[1]);
        if (DataCore.GetInstance().MainPlayerGuid != strMessages[0])
        {
            switch (type)
            {
                case (int)NetType.Init:
                    DataCore.GetInstance().MainPlayerGuid = strMessages[0];
                    string strMessage = DataCore.GetInstance().MainPlayerGuid + NetType.EnterSence.ToString() + "," + "Cube";
                    SendMessage(strMessage);
                    break;
                case (int)NetType.EnterSence:
                    string prefavName = strMessages[2];
                    if (NetObj == null)
                        DataCore.GetInstance().AddObjPathList(prefavName);
                    break;
                case (int)NetType.Move:
                    string [] posStrs = strMessages[2].Split('.');
                    Vector3 pos = new Vector3( int.Parse(posStrs[0]), int.Parse(posStrs[1]),int.Parse(posStrs[2]));
                    DataCore.GetInstance().SetPrefabPos(pos);
                    break;
                default:
                    break;
            }
        }
    }

    public void SendMessage(string message)
    {
        string guid = DataCore.GetInstance().MainPlayerGuid;
        string sendMessage = guid + "," + message;
        SendNetMessage(sendMessage);
    }
}
