using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;


    class TankTest :MonoBehaviour
    {
        void Start()
        {
            GameObject tankObj = new GameObject("myTank");
            CtrlTank ctrlTank = tankObj.AddComponent<CtrlTank>();
            ctrlTank.Init("tankPrefab");
            tankObj.AddComponent<CameraFollow>();

            GameObject OhterTankObj = new GameObject("otherTank");
            CtrlTank OhterCtrlTank = OhterTankObj.AddComponent<CtrlTank>();
            OhterCtrlTank.Init("tankPrefab");
            OhterCtrlTank.transform.position= new Vector3(0,10,30);
        }

        void Update()
        {
            ObjectPool.GetInstance().Update();
        }
    }

