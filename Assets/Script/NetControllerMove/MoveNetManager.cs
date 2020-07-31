using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class MoveNetManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    byte [] receiveMessage = new byte[1024];
    public void StartConnect()
    {
        Socket socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
        socket.Connect("127.0.0.1", 9999);
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

    public void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket) ar.AsyncState;
            int count =  socket.EndReceive(ar);
            string message = System.Text.Encoding.Default.GetString(receiveMessage, 0, count);
            Pack(message);
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
    public void UnPack()
    {

    }

    public void SendPosMessage()
    {

    }
}
