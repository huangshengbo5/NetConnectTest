using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameWorkTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        FrameWorkNetManager.AddEventListener(NetEvent.ConnectSucc, OnConnectSucc);
        FrameWorkNetManager.AddEventListener(NetEvent.ConnectFail, OnConnectFail);
        FrameWorkNetManager.AddEventListener(NetEvent.Close, OnClose);
	}


    public static void OnConnectSucc(string str)
    {
        Debug.Log("Connect Successful");
    }

    public static void OnConnectFail(string str)
    {
        Debug.Log("Connect Fail");
    }

    public static void OnClose(string str)
    {
        Debug.Log("Connect Close");
    }

    //点击连接
    public void ConnectClick()
    {
        FrameWorkNetManager.Connect("127.0.0.1",8888);
    }

    public void OnCloseClick()
    {
        FrameWorkNetManager.Close();
    }
}
