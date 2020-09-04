using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Layer
{
    Panel,
    Tip,
}
public static class PanelManager  {

    private static Dictionary<Layer,Transform>layers = new Dictionary<Layer, Transform>();

    public static Dictionary<string,BasePanel>panels = new Dictionary<string, BasePanel>();

    public static Transform root;
    public static Transform canvas;


    public static void Init()
    {
        root = GameObject.Find("Root").transform;
        canvas = root.transform.Find("Canvas");
        Transform panel = canvas.Find("Panel");
        Transform tip = canvas.Find("Tip");
        layers.Add(Layer.Panel,panel);
        layers.Add(Layer.Tip,tip);
    }

    public static void Open<T>(params object[] para) where T : BasePanel
    {
        string name = typeof(T).ToString();
        if (panels.ContainsKey(name))
        {
            return;
        }

        BasePanel panel = root.gameObject.AddComponent<T>();
        panel.OnInit();
        panel.Init();

        Transform layer = layers[panel.layer];
        panel.skin.transform.SetParent(layer,false);
        panels.Add(name,panel);
        panel.OnShow(para);
    }

    public static void Close(string name)
    {
        if (!panels.ContainsKey(name))
        {
            return;
        }

        BasePanel panel = panels[name];
        panel.OnClose();
        panels.Remove(name);
        GameObject.Destroy(panel.skin);
        Component.Destroy(panel);
    }
    
}
