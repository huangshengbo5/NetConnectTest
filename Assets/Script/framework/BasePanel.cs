using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{


    public string skinPath;

    public GameObject skin;

    public void Init()
    {
        GameObject obj = ResManager.LoadPrefab(skinPath);
        skin = Instantiate(obj);
    }

    public void Close()
    {
        string name = this.GetType().ToString();
        PanelManager.Close(name);
    }

    public virtual void OnInit()
    {

    }

    public virtual void OnShow(params object[]para)
    {

    }

    public virtual void OnClose()
    {

    }
}
