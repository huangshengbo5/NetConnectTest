using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.UI;

public class TipPanel : BasePanel
{
    private Text text;
    private Button okBtn;

    public override void OnInit()
    {
        skinPath = "TipPanel";
        layer = Layer.Tip;
    }

    public override void OnShow(params object[] para)
    {
        text = skin.transform.Find("Text").GetComponent<Text>();
        okBtn = skin.transform.Find("OkBtn").GetComponent<Button>();
        if (para.Length == 1)
        {
            text.text = para[0].ToString();
        }
        okBtn.onClick.AddListener(OnOkClick);
    }

    public override void OnClose()
    {
        
    }

    public void OnOkClick()
    {
        Close();
    }
}
