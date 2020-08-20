using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MainAccess : MonoBehaviour
{
    public GameObject humanPrefab;
    
    private BaseHuman myHuman;

    public Dictionary<string, BaseHuman> otherHumans = new Dictionary<string, BaseHuman>();
	// Use this for initialization
	void Start () {
		//NetManager.AddListener("Enter",OnEnter);
        NetManager.AddListener("Move",OnMove);
        NetManager.AddListener("Leave",OnLeave);
        NetManager.AddListener("List",OnList);
        NetManager.AddListener("Attack",OnAttack);
        NetManager.Connect("127.0.0.1",9999);

        GameObject obj = Instantiate(humanPrefab);
        float x = Random.Range(-5, 5);
        float z = Random.Range(-5, 5);
        obj.transform.position = new Vector3(x,0,z);
        myHuman = obj.AddComponent<CtrlHuman>();

        myHuman.desc = NetManager.GetDesc();

        Vector3 pos = obj.transform.position;
        Vector3 eul = obj.transform.eulerAngles;
        string sendStr = "Enter|";
        sendStr += NetManager.GetDesc() + ",";
        sendStr += pos.x + ",";
        sendStr += pos.y + ",";
        sendStr += pos.z + ",";
        sendStr += eul.y;
        NetManager.Send(sendStr);

        NetManager.Send("List|");
    }
	
	// Update is called once per frame
	void Update () {
		NetManager.Update();
	}

    public void OnEnter(string str)
    {
        string[] enterStr = str.Split(',');
        string enterDesc = enterStr[0];
        if (enterDesc == NetManager.GetDesc()) return;
        Debug.Log("Other OnEnter:  " + str);
        float posX = float.Parse(enterStr[1]);
        float posY = float.Parse(enterStr[2]);
        float posZ = float.Parse(enterStr[3]);
        float eulX = float.Parse(enterStr[4]);

        var syncHuman = Instantiate(humanPrefab);
        SyncHuman sync = syncHuman.AddComponent<SyncHuman>();
        syncHuman.transform.position = new Vector3(posX,posY,posZ);
        syncHuman.transform.eulerAngles = new Vector3(0,eulX,0);
        syncHuman.transform.rotation = Quaternion.identity;
        sync.desc = enterDesc;
        otherHumans.Add(enterDesc, sync);
    }

    public void OnMove(string str)
    {
        Debug.Log("OnMove" + str);

        string[] split = str.Split(',');
        string desc = split[0];
        float x = float.Parse(split[1]);
        float y = float.Parse(split[2]);
        float z = float.Parse(split[3]);
        if (!otherHumans.Keys.Contains(desc))
        {
            return;
        }
        BaseHuman baseHuman = otherHumans[desc];
        Vector3 targetPos = new  Vector3(x,y,z);
        baseHuman.MoveTo(targetPos);
    }
    public void OnLeave(string str)
    {
        Debug.Log("OnLeave" + str);
        string[] split = str.Split(',');
        string desc = split[0];
        if (!otherHumans.Keys.Contains(desc))
        {
            return;
        }
        GameObject obj = otherHumans[desc].gameObject;
        Destroy(obj);
    }

    public void OnList(string str)
    {
        Debug.Log("OnList:  " +str);
        string [] split = str.Split(',');
        int count = (split.Length - 1) / 6;
        for (int i = 0; i < count; i++)
        {
            string desc = split[i * 6 + 0];
            float x = float.Parse(split[i * 6 + 1]);
            float y = float.Parse(split[i * 6 + 2]);
            float z = float.Parse(split[i * 6 + 3]);
            float eulY = float.Parse(split[i * 6 + 4]);
            int hp = int.Parse(split[i * 6 + 5]);
            if (desc == NetManager.GetDesc())
            {
                continue;
            }
            GameObject obj = Instantiate(humanPrefab);
            obj.transform.position =  new Vector3(x,y,z);
            obj.transform.eulerAngles = new Vector3(0,eulY,0);
            BaseHuman otherHuman = obj.AddComponent<SyncHuman>();
            otherHuman.desc = desc;
            otherHumans.Add(desc, otherHuman);
            
        }
    }

    public void OnAttack(string str)
    {
        Debug.Log("OnAttack: " + str);
        string[] split = str.Split(',');
        string desc = split[0];
        float eulY = float.Parse(split[1]);
        if (!otherHumans.ContainsKey(desc))
        {
            return;
        }
        SyncHuman h = (SyncHuman) otherHumans[desc];
        h.SyncAttack(eulY);
    }

    public void OnDie(string Args)
    {
        Debug.Log("OnDie: "+ Args);
        string[] split = Args.Split(',');
        //string attDesc = split[0];
        string hitDesc = split[0];
        if (hitDesc == myHuman.desc)
        {
            Debug.Log("GAME OVER!!!");
        }
        if (!otherHumans.ContainsKey(hitDesc))
        {
            return;
        }
        SyncHuman syncHuman = (SyncHuman) otherHumans[hitDesc];
        syncHuman.gameObject.SetActive(false);
    }
}
