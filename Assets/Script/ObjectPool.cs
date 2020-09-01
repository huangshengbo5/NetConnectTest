using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

public class ObjectPool
{

    public static ObjectPool Instance;
    //public static GameObject obj = new GameObject();

    public static ObjectPool GetInstance()
    {
        if (Instance == null)
        {
            Instance = new ObjectPool();
        }
        return Instance;
    }

    public class PoolItem
    {
        private GameObject gameObj;
        public float activeTime;

        public GameObject GameObj
        {
            get { return gameObj; }
            set { gameObj = value; }
        }
        private bool isActive;

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        
        public void DoActive()
        {
            if (gameObj)
            {
                gameObj.SetActive(true);
                isActive = true;
                activeTime = Time.time;
            }
        }

        public void DoDeactive()
        {
            if (gameObj)
            {
                gameObj.SetActive(false);
                isActive = false;
            }
        }
    }

    public List<PoolItem> objectList = new List<PoolItem>();
    public List<PoolItem> needRemoveobjectList = new List<PoolItem>();

    public int limitCount = 10;
    public float poolItemLife = 5f;

    public  GameObject CreateObj(string path)
    {
        PoolItem poolItem = new PoolItem();
        GameObject skin = null;
        if (objectList.Count < 10)
        {
            GameObject skinRes = ResManager.LoadPrefab(path);
            skin = GameObject.Instantiate(skinRes);
            poolItem.GameObj = skin;
            objectList.Add(poolItem);
        }
        else
        {
            foreach (var item in objectList)
            {
                if (item.IsActive == false)
                {
                    poolItem = item;
                    skin = poolItem.GameObj;
                }
            }
        }
        poolItem.DoActive();
        return skin;
    }

    public void Update()
    {
        if (objectList.Count>limitCount)
        {
            for (int i = limitCount-1; i < objectList.Count; i++)
            {
                if (objectList[i].IsActive == false)
                {
                    GameObject.Destroy(objectList[i].GameObj);
                    needRemoveobjectList.Add(objectList[i]);
                }
            }
        }
        for (int i = 0; i < needRemoveobjectList.Count; i++)
        {
            objectList.Remove(needRemoveobjectList[i]);
        }

        for (int i = 0; i < objectList.Count; i++)
        {
            if (objectList[i].IsActive)
            {
                if (Time.time - objectList[i].activeTime > poolItemLife)
                {
                    objectList[i].DoDeactive();
                }
            }
        }
    }
}
