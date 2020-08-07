using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAccess : MonoBehaviour
{
    public GameObject humanPrefab;
    
    private BaseHuman myHuman;

    public Dictionary<string, BaseHuman> otherHumans = new Dictionary<string, BaseHuman>();
	// Use this for initialization
	void Start () {
		NetManager.AddListener("Enter",OnEnter);
        NetManager.AddListener("Move",OnMove);
        NetManager.AddListener("Leave",OnLeave);
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

    }
    public void OnLeave(string str)
    {
        Debug.Log("OnLeave" + str);
    }
}
