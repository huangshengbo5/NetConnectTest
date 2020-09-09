using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanel : BasePanel {

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
        scoreText = skin.transform.Find("InfoPanel/scoreText").GetComponent<Text>();
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
        FrameWorkNetManager.AddMsgListener("MsgGetAchieve",OnMsgGetAchieve);

    }

    public void OnCreateClick()
    {

    }

    public void OnReflashClick()
    {

    }

    public void OnMsgGetAchieve(FrameWorkMsgBase str)
    {

    }
}
