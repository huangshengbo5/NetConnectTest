using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : BasePanel
{

    public Image winImage;
    private Image lostImage;
    private Button okBtn;

    public override void OnInit()
    {
        skinPath = "ResultPanel";
        layer = Layer.Tip;
    }

    public override void OnShow(params object[] para)
    {
        winImage = skin.transform.Find("WinImage").GetComponent<Image>();
        lostImage = skin.transform.Find("LostImage").GetComponent<Image>();
        okBtn = skin.transform.Find("OkBtn").GetComponent<Button>();
        okBtn.onClick.AddListener(OnOkClick);
        if (para.Length==1)
        {
            bool isWin = (bool) para[0];
            winImage.gameObject.SetActive(isWin);
            lostImage.gameObject.SetActive(!isWin);
        }
    }

    public void OnOkClick()
    {
        PanelManager.Open<RoomPanel>();
        Close();
    }
}
