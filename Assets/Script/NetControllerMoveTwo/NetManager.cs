using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System;
using System.Net.Sockets;

public class NetManager
{
    private static Socket socket;

    static byte[] readBuff = new byte[1024];

    public delegate void MsgListener(string str);
    private static Dictionary<string ,MsgListener> listeners = new Dictionary<string, MsgListener>();

    static List<string>msgList = new List<string>();

    //添加监听
    public static void AddListener(string msgName, MsgListener listener)
    {
        listeners.Add(msgName, listener);
    }

    public static string GetDesc()
    {
        if (socket == null) return "";
        if (!socket.Connected) return "";
        return socket.LocalEndPoint.ToString();
    }

    public static void Connect(string ip, int port)
    {
        socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
        socket.Connect(ip,port);
        socket.BeginReceive(readBuff, 0, 1024, 0, ReceiveCallBack,socket);
    }

    public static void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            int count = socket.EndReceive(ar);
            string receStr = System.Text.Encoding.Default.GetString(readBuff, 0, count);
            msgList.Add(receStr);
            socket.BeginReceive(readBuff, 0, 1024, 0, ReceiveCallBack, socket);
        }
        catch (SocketException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static void Send(string sendStr)
    {
        if (socket == null) return;
        if (!socket.Connected) return;
        byte[] sendBytes = System.Text.Encoding.Default.GetBytes(sendStr);
        socket.Send(sendBytes);
    }

    public static void Update()
    {
        if (msgList.Count <=0)
        {
            return;
        }
        string msgStr = msgList[0];
        msgList.RemoveAt(0);
        string[] spilt = msgStr.Split('|');
        string msgName = spilt[0];
        string msgArgs = spilt[1];
        if (listeners.ContainsKey(msgName))
        {
            listeners[msgName](msgArgs);
        }
    }
}
