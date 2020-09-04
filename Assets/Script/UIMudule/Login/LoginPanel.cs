using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : BasePanel
{

    private InputField idInput;
    private InputField pwInput;

    private Button loginBtn;
    private Button regBtn;

    public override void OnInit()
    {
        skinPath = "LoginPanel";
        layer = Layer.Panel;
    }

    public override void OnShow(params object[] args)
    {
        idInput = skin.transform.Find("IdInput").GetComponent<InputField>();
        pwInput = skin.transform.Find("PwInput").GetComponent<InputField>();
        loginBtn = skin.transform.Find("LoginBtn").GetComponent<Button>();
        regBtn = skin.transform.Find("RegisterBtn").GetComponent<Button>();
        loginBtn.onClick.AddListener(OnLoginClick);
        regBtn.onClick.AddListener(OnRegClick);

        //网络协议监听
        FrameWorkNetManager.AddMsgListener("MsgLogin", OnMsgLogin);

        //网络事件监听
        FrameWorkNetManager.AddEventListener(NetEvent.ConnectSucc, OnConnectSucc);
        FrameWorkNetManager.AddEventListener(NetEvent.ConnectFail, OnConnectFail);
    }

    public override void OnClose()
    {

        FrameWorkNetManager.RemoveMsgListenr("MsgLogin", OnMsgLogin);

        FrameWorkNetManager.RemoveEventListener(NetEvent.ConnectSucc, OnConnectSucc);
        FrameWorkNetManager.RemoveEventListener(NetEvent.ConnectFail, OnConnectFail);
    }

    public void OnLoginClick()
    {
        if (idInput.text == "" || pwInput.text == "")
        {
            PanelManager.Open<TipPanel>("用户名和密码不能为空");
            return;
        }

        MsgLogin msgLogin = new MsgLogin();
        msgLogin.id = idInput.text;
        msgLogin.pw = pwInput.text;
        FrameWorkNetManager.Send(msgLogin);
    }

    public void OnRegClick()
    {

    }

    public void OnMsgLogin(FrameWorkMsgBase msgBase)
    {
        MsgLogin msgLogin = (MsgLogin) msgBase;
        if (msgLogin.result == 0) //登陆成功
        {
            Debug.Log("登录成功");
            //进入游戏，并增加坦克
            GameObject tankObj = new GameObject("myTank");
            CtrlTank ctrlTank = tankObj.AddComponent<CtrlTank>();
            ctrlTank.Init("tankPrefab");
            tankObj.AddComponent<CameraFollow>();
            Close();
        }
        else
        {
            PanelManager.Open<TipPanel>("登录失败");
        }
    }

    public void OnConnectSucc(string str)
    {
        Debug.Log("OnConnectSuccessful");
    }

    public void OnConnectFail(string str)
    {
        PanelManager.Open<TipPanel>(str);
    }

}
