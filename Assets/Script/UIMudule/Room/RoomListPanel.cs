using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RoomListPanel : BasePanel {

   

    //账号文本
    private Text idText;

    //战绩文本
    private Text scoreText;
    //创建房间按钮
    private Button createButton;
    //刷新列表按钮
    private Button reflashButton;
    //列表容器
    private Transform content;
    //房间物体
    private GameObject roomObj;

    public override void OnShow(params object[] para)
    {
        idText = skin.transform.Find("InfoPanel/IdText").GetComponent<Text>();
        scoreText = skin.transform.Find("InfoPanel/ScoreText").GetComponent<Text>();
        createButton = skin.transform.Find("CtrlPanel/CreateButton").GetComponent<Button>();
        reflashButton = skin.transform.Find("CtrlPanel/ReflashButton").GetComponent<Button>();

        content = skin.transform.Find("ListPanel/Scroll View/Viewport/Content");
        roomObj = skin.transform.Find("Room").gameObject;

        //按钮事件
        createButton.onClick.AddListener(OnCreateClick);
        reflashButton.onClick.AddListener(OnReflashClick);
        //不激活房间
        roomObj.SetActive(false);
        //显示id
        idText.text = GameMain.id;

        //协议监听
        FrameWorkNetManager.AddMsgListener("MsgGetAchieve", OnMsgGetAchieve);
        FrameWorkNetManager.AddMsgListener("MsgGetRoomList",OnMsgGetRoomList);
        FrameWorkNetManager.AddMsgListener("MsgCreateRoom", OnMsgCreateRoom);
        FrameWorkNetManager.AddMsgListener("MsgEnterRoom", OnMsgEnterRoom);

        //发送查询

        MsgGetAchieve msgGetAchieve = new MsgGetAchieve();
        FrameWorkNetManager.Send(msgGetAchieve);
        MsgGetRoomList msgGetRoomList = new MsgGetRoomList();
        FrameWorkNetManager.Send(msgGetRoomList);

    }

    public override void OnInit()
    {
        skinPath = "RoomListPanel";
        layer = Layer.Panel;
    }


    public override void OnClose()
    {
        FrameWorkNetManager.RemoveMsgListenr("MsgGetAchieve", OnMsgGetAchieve);
        FrameWorkNetManager.RemoveMsgListenr("MsgGetRoomList", OnMsgGetRoomList);
        FrameWorkNetManager.RemoveMsgListenr("MsgCreateRoom", OnMsgCreateRoom);
        FrameWorkNetManager.RemoveMsgListenr("MsgEnterRoom", OnMsgEnterRoom);
    }

    public void OnCreateClick()
    {
        MsgCreateRoom msg = new MsgCreateRoom();
        FrameWorkNetManager.Send(msg);
    }

    public void OnReflashClick()
    {
        MsgGetRoomList msg = new MsgGetRoomList();
        FrameWorkNetManager.Send(msg);
    }

    public void OnMsgGetAchieve(FrameWorkMsgBase str)
    {
        MsgGetAchieve msg = (MsgGetAchieve) str;
        scoreText.text = msg.win + "胜" + msg.lost + "负";
    }

    public void OnMsgGetRoomList(FrameWorkMsgBase str)
    {
        MsgGetRoomList msg = (MsgGetRoomList) str;
        for (int i = content.childCount-1; i >=0; i--)
        {
            GameObject o = content.GetChild(i).gameObject;
            Destroy(o);
        }

        if (msg.rooms == null)
        {
            return;
        }

        for (int i = 0; i < msg.rooms.Length; i++)
        {
            GenerateRoom(msg.rooms[i]);
        }
    }

    public void OnMsgCreateRoom(FrameWorkMsgBase str)
    {
        MsgCreateRoom msg = (MsgCreateRoom) str;
        if (msg.result == 0)
        {
            PanelManager.Open<TipPanel>("创建成功");
            PanelManager.Open<RoomPanel>();
            Close();
        }
        else
        {
            PanelManager.Open<TipPanel>("创建房间失败");
        }
    }

    public void OnMsgEnterRoom(FrameWorkMsgBase str)
    {
        MsgEnterRoom msg = (MsgEnterRoom) str;
        if (msg.result == 0)
        {
            PanelManager.Open<RoomPanel>();
            Close();
        }
        else
        {
            PanelManager.Open<TipPanel>("进入房间失败");
        }
    }

    public void GenerateRoom(RoomInfo roomInfo)
    {
        GameObject o = Instantiate(roomObj);
        o.transform.SetParent(content);
        o.SetActive(true);
        o.transform.localScale = Vector3.one;

        Transform trans = o.transform;
        Text idText = trans.Find("IdText").GetComponent<Text>();
        Text countText = trans.Find("CountText").GetComponent<Text>();
        Text statusText = trans.Find("StatusText").GetComponent<Text>();
        Button btn = trans.Find("JoinButton").GetComponent<Button>();

        idText.text = roomInfo.id.ToString();
        countText.text = roomInfo.count.ToString();
        statusText.text = roomInfo.status == 0 ? "准备中" : "战斗中";

        btn.name = idText.text;
        btn.onClick.AddListener(delegate() { OnJoinClick(btn.name);});
    }

    public void OnJoinClick(string idString)
    {
        MsgEnterRoom msg = new MsgEnterRoom();
        msg.id = int.Parse(idString);
        FrameWorkNetManager.Send(msg);
    }
}
