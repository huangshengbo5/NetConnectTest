using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPanel : BasePanel
{


    private InputField idInput;

    private InputField pwInput;

    private InputField repInput;

    private Button regBtn;

    private Button closeBtn;

    public override void OnInit()
    {
        skinPath = "RegisterPanel";
        layer = Layer.Panel;
    }

    public override void OnShow(params object[] args)
    {
        idInput = skin.transform.Find("IdInput").GetComponent<InputField>();
        pwInput = skin.transform.Find("PwInput").GetComponent<InputField>();
        repInput = skin.transform.Find("RepInput").GetComponent<InputField>();
        regBtn = skin.transform.Find("RegisterBtn").GetComponent<Button>();
        closeBtn = skin.transform.Find("CloseBtn").GetComponent<Button>();
        regBtn.onClick.AddListener(OnRegClick);
        closeBtn.onClick.AddListener(OnCloseClick);
        FrameWorkNetManager.AddMsgListener("MsgRegister",OnMsgRegister);
    }

    public override void OnClose()
    {
        FrameWorkNetManager.RemoveMsgListenr("MsgRegister",OnMsgRegister);
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //注册
    public void OnRegClick()
    {
        if (idInput.text == "" || pwInput.text =="")
        {
            PanelManager.Open<TipPanel>("用户名和密码不能为空");
            return;
        }

        if (repInput.text != pwInput.text)
        {
            PanelManager.Open<TipPanel>("两次输入的密码不一致");
            return;
        }
        MsgRegister msgRegister = new MsgRegister();
        msgRegister.id = idInput.text;
        msgRegister.pw = idInput.text;
        FrameWorkNetManager.Send(msgRegister);
    }


    public void OnCloseClick()
    {
        Close();
    }

    public void OnMsgRegister(FrameWorkMsgBase msgBase)
    {
        MsgRegister msgReg = (MsgRegister) msgBase;
        if (msgReg.result == 0)
        {
            Debug.Log("注册成功");
            PanelManager.Open<TipPanel>("注册成功");
            Close();
        }
        else
        {
            PanelManager.Open<TipPanel>("注册失败");
        }
    }
}
