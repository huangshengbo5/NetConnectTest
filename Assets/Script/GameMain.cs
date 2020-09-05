using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{

    public static string id = "";

	// Use this for initialization
	void Start () {
		FrameWorkNetManager.AddEventListener(NetEvent.Close,OnConnectClose);
        FrameWorkNetManager.AddMsgListener("MsgKick",OnMsgKick);

        PanelManager.Init();
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
}
