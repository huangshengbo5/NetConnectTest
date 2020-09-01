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
        }

        void Update()
        {
            ObjectPool.GetInstance().Update();
        }
    }

