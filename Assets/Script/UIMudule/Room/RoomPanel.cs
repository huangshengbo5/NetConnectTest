using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanel : BasePanel
{


    private Button startButton;
    private Button closeButton;
    private Transform content;
    private GameObject playerObj;

    public override void OnInit()
    {
        skinPath = "RoomPanel";
        layer = Layer.Panel;
    }

    public override void OnShow(params object[] para)
    {
        startButton = skin.transform.Find("CtrlPanel/StartButton").GetComponent<Button>();
        closeButton = skin.transform.Find("CtrlPanel/CloseButton").GetComponent<Button>();
        content = skin.transform.Find("ListPanel/Scroll View/Viewport/Content");
        playerObj = skin.transform.Find("Player").gameObject;
        playerObj.SetActive(false);
        startButton.onClick.AddListener(OnStartClick);
        closeButton.onClick.AddListener(OnCloseClick);

        FrameWorkNetManager.AddMsgListener("MsgGetRoomInfo",OnMsgGetRoomInfo);
        FrameWorkNetManager.AddMsgListener("MsgLeaveRoom", OnMsgLeaveRoom);
        FrameWorkNetManager.AddMsgListener("MsgStartBattle", OnMsgStartBattle);

        MsgGetRoomInfo msg = new MsgGetRoomInfo();
        FrameWorkNetManager.Send(msg);
    }

    public void OnStartClick()
    {
        MsgStartBattle msg = new MsgStartBattle();
        FrameWorkNetManager.Send(msg);
    }

    public void OnCloseClick()
    {
        MsgLeaveRoom msg = new MsgLeaveRoom();
        FrameWorkNetManager.Send(msg);
    }

    public override void OnClose()
    {
        FrameWorkNetManager.RemoveMsgListenr("MsgGetRoomInfo", OnMsgGetRoomInfo);
        FrameWorkNetManager.RemoveMsgListenr("MsgLeaveRoom", OnMsgLeaveRoom);
        FrameWorkNetManager.RemoveMsgListenr("MsgStartBattle", OnMsgStartBattle);
    }

    public void OnMsgGetRoomInfo(FrameWorkMsgBase str)
    {
        MsgGetRoomInfo msg = (MsgGetRoomInfo) str;
        for (int i = content.childCount-1; i >=0; i--)
        {
            GameObject o = content.GetChild(i).gameObject;
            Destroy(o);
        }

        if (msg.players == null)
        {
            return;
        }

        for (int i = 0; i < msg.players.Length; i++)
        {
            GeneratePlayerInfo(msg.players[i]);
        }
    }

    public void OnMsgLeaveRoom(FrameWorkMsgBase str)
    {
        MsgLeaveRoom msg = (MsgLeaveRoom) str;
        if (msg.result==0)
        {
            PanelManager.Open<TipPanel>("退出房间");
            PanelManager.Open<RoomListPanel>();
            Close();
        }
        else
        {
            PanelManager.Open<TipPanel>("退出房间失败");
        }
    }

    public void OnMsgStartBattle(FrameWorkMsgBase str)
    {
        MsgStartBattle msg = (MsgStartBattle) str;
        if (msg.result == 0)
        {
            Close();
        }
        else
        {
            PanelManager.Open<TipPanel>("开战失败，两队至少都需要一名玩家，只有队长可以开始战斗");
        }
    }

    public void GeneratePlayerInfo(PlayerInfo playerInfo)
    {
        GameObject o = Instantiate(playerObj);
        o.transform.SetParent(content);
        o.SetActive(true);
        o.transform.localScale = Vector3.one;

        Transform trans = o.transform;
        Text idText = trans.Find("IdText").GetComponent<Text>();
        Text campText = trans.Find("CampText").GetComponent<Text>();
        Text scoreText = trans.Find("ScoreText").GetComponent<Text>();

        idText.text = playerInfo.id;
        campText.text = playerInfo.camp == 1 ? "红" : "蓝";
        if (playerInfo.isOwner == 1)
        {
            campText.text = campText.text + "!";
        }

        scoreText.text = playerInfo.win + "胜" + playerInfo.lost + "负";
    }
}
