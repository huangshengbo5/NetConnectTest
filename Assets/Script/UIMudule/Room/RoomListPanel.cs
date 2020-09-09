using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListPanel : BasePanel {

    public override void OnInit()
    {
        skinPath = "RoomListPanel";
        layer = Layer.Panel;
    }

    public override void OnShow(params object[] args)
    {

    }

    public override void OnClose()
    {

    }
}
