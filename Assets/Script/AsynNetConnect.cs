using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;

public class AsynNetConnect : MonoBehaviour {

    public Button Btn_Connect;
    public Button Btn_Send;
    public InputField Input_Text;
    public Text Out_Text;
    private Socket socket;
    byte [] readBuff = new byte[1024];
    private string recvStr = "";


    public void Connection()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //socket.Connect("127.0.0.1", 9999);
        socket.BeginConnect("127.0.0.1", 9999, ConnectionCallBack, socket);
    }

    public void ConnectionCallBack(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket) ar.AsyncState;
            socket.EndConnect(ar);
            Debug.Log("Socket Connect Success");
            socket.BeginReceive(readBuff, 0, 1024, 0, ReceiveCallBack,socket);
        }
        catch (SocketException e)
        {
            Debug.Log("Socket Connect Fail " + e);
            //Console.WriteLine(e);
            throw;
        }
    }

    public void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            int count = socket.EndReceive(ar);

            string curRecvStr = System.Text.Encoding.Default.GetString(readBuff, 0, count);
            recvStr = curRecvStr + '\n' + recvStr;
           
            Debug.Log("Socket Connect Success");
            socket.BeginReceive(readBuff, 0, 1024, 0, ReceiveCallBack,socket);

        }
        catch (SocketException e)
        {
            Debug.Log("Socket Receive Fail "+ e);
            //Console.WriteLine(e);
            throw;
        }
    }

    public void Send()
    {
        string sendStr = Input_Text.text;
        byte[] sendBytes = System.Text.Encoding.Default.GetBytes(sendStr);
        //socket.Send(sendBytes);
        socket.BeginSend(sendBytes, 0, sendBytes.Length, 0, SendCallBack, socket);
        //byte[] readBuff = new byte[1024];
        //int count = socket.Receive(readBuff);
        //string recvStr = System.Text.Encoding.Default.GetString(readBuff, 0, count);
        //Out_Text.text = recvStr;
        //socket.Close();
    }

    void SendCallBack(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket) ar.AsyncState;
            socket.EndConnect(ar);
            Debug.Log("Socket Send Success");
        }
        catch (SocketException s)
        {
            Debug.Log("Socket Send Fail" + s);
            //Console.WriteLine(s);
            throw;
        }
    }

    void Update()
    {
        Out_Text.text = recvStr;
    }
}
