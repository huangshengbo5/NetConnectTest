using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataCore
{

    public static DataCore Instance;

    public static DataCore GetInstance()
    {
        if (Instance == null)
        {
            Instance = new  DataCore();
        }
        return Instance;
    }

    public string MainPlayerGuid;

    public Dictionary<string ,GameObject> m_PrefabList = new Dictionary<string, GameObject>();

    public  List<GameObject> m_objList = new List<GameObject>();
    public string name = "";
    public Vector3 curPrefabpos;

    public void AddObjPathList(string name = "",GameObject obj = null)
    {
        if (!m_PrefabList.Keys.Contains(name))
        {
            m_PrefabList.Add(name, obj);
        }
    }

    public void SetPrefabPos(Vector3 pos)
    {
        curPrefabpos = pos;
    }

    public void Update()
    {
        if (name == "")
        {
            foreach (var item in m_PrefabList)
            {
                if (item.Value == null)
                {
                    name = item.Key;
                    break;
                }
            }
        }

        if (name!= "")
        {
            var obj = m_PrefabList[name];
            if (obj == null)
            {
                obj = null;
                obj = (GameObject)GameObject.Instantiate(Resources.Load(name));
                obj.AddComponent<MoveNetPlayer>();
                obj.transform.position = Vector3.zero;
                obj.transform.localScale = Vector3.one;
                m_objList.Add(obj);
            }
            m_PrefabList.Remove(name);
            name = "";
        }

        if (curPrefabpos != Vector3.zero)
        {
            foreach (var VARIABLE in m_objList)
            {
                MoveNetPlayer player = VARIABLE.GetComponent<MoveNetPlayer>();
                player.SetNetPostion(curPrefabpos);
            }
            curPrefabpos = Vector3.zero;
        }
    }
}
