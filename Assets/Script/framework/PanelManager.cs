using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PanelManager  {


    public enum Layer
    {
        Panel,
        Tip,
    }

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

    }
    public static void Close(string name)
    {

    }
    
}
