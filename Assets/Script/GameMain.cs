using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{

    public static string id = "";

	// Use this for initialization
	void Start () {
		FrameWorkNetManager.AddEventListener(NetEvent.Close,OnConnectClose);
        FrameWorkNetManager.AddEventListener(NetEvent.ConnectSucc, OnConnectSucc);
        FrameWorkNetManager.AddEventListener(NetEvent.ConnectFail, OnConnectFail);
        FrameWorkNetManager.AddMsgListener("MsgKick",OnMsgKick);
        FrameWorkNetManager.Connect("127.0.0.1",8888);
        PanelManager.Init();
        BattleManager.Init();
        PanelManager.Open<LoginPanel>();
	}
	
	// Update is called once per frame
	void Update () {
		FrameWorkNetManager.Update();
	}

    public void OnConnectClose(string str)
    {
        Debug.Log("断开连接");
    }

    public void OnMsgKick(FrameWorkMsgBase msgBase)
    {
        PanelManager.Open<TipPanel>("被踢下线");
    }


    public static void OnConnectSucc(string str)
    {
        Debug.Log("Connect Successful");
    }

    public static void OnConnectFail(string str)
    {
        Debug.Log("Connect Fail");
    }
}
