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
    public void UnPack(string message)
    {
        string[] strMessages = message.Split(',');
        string postName = strMessages[0];
        if (postName == Uitil.GetHostIPAdress())
        {
            int type = int.Parse(strMessages[1]);
            if (type == (int)NetType.EnterSence)
            {
                string prefavName = strMessages[2];
                if (NetObj == null)
                {
                    NetObj = (GameObject)GameObject.Instantiate(Resources.Load(prefavName));
                    NetObj.AddComponent<MoveNetPlayer>();
                    NetObj.transform.position = Vector3.zero;
                    NetObj.transform.localScale = Vector3.one;
                }
            }
            else if (type == (int)NetType.Move)
            {
                string [] posStrs = strMessages[2].Split('.');
                Vector3 pos = new Vector3( int.Parse(posStrs[0]), int.Parse(posStrs[1]),int.Parse(posStrs[2]));
                MoveNetPlayer player = NetObj.GetComponent<MoveNetPlayer>();
                player.SetNetPostion(pos);
            }
            else
            {
                GameObject.Destroy(NetObj); 
            }
        }
    }

    public void SendMessage(string message)
    {
        string ipPos = Uitil.GetHostIPAdress();
        string sendMessage = ipPos + "," + message;
        SendNetMessage(sendMessage);
    }
}
