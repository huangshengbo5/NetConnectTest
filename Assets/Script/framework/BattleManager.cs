using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager  {

	 public  static Dictionary<string ,BaseTank> tanks = new Dictionary<string, BaseTank>();


     public static void Init()
     {
         FrameWorkNetManager.AddMsgListener("MsgEnterBattle",OnMsgEnterBattle);
         FrameWorkNetManager.AddMsgListener("MsgBattleResult", OnMsgBattleResult);
         FrameWorkNetManager.AddMsgListener("MsgLeaveBattle", OnMsgLeaveBattle);
         FrameWorkNetManager.AddMsgListener("MsgSyncTank", OnMsgSyncTank);
         FrameWorkNetManager.AddMsgListener("MsgFire", OnMsgFire);
         FrameWorkNetManager.AddMsgListener("MsgHit", OnMsgHit);
     }

     public static void OnMsgSyncTank(FrameWorkMsgBase msgBase)
     {
         MsgSyncTank msg = (MsgSyncTank) msgBase;
         if (msg.id == GameMain.id)
         {
             return;
         }

         SyncTank tank = (SyncTank) GetTank(msg.id);
         if (tank == null)
         {
             return;
         }

         tank.SyncPos(msg);
     }

     public static void OnMsgFire(FrameWorkMsgBase msgBase)
     {
         MsgFire msg = (MsgFire) msgBase;
         if (msg.id == GameMain.id)
         {
             return;
         }

         SyncTank tank = (SyncTank) GetTank(msg.id);
         if (tank == null)
         {
             return;
         }

         tank.SyncFire(msg);
     }

     public static void OnMsgHit(FrameWorkMsgBase msgBase)
     {
         MsgHit msg = (MsgHit) msgBase;
         BaseTank tank = GetTank(msg.targetId);
         if (tank == null)
         {
             return;
         }
         tank.Attacked(msg.damage);
     }

     public static void OnMsgEnterBattle(FrameWorkMsgBase msgBase)
     {
         MsgEnterBattle msg = (MsgEnterBattle) msgBase;
         EnterBattle(msg);
     }

     public static void OnMsgBattleResult(FrameWorkMsgBase msgBase)
     {
         MsgBattleResult msg = (MsgBattleResult) msgBase;
         bool isWin = false;
         BaseTank tank = GetCtrlTank();
         if (tank !=null&& tank.camp == msg.winCamp)
         {
             isWin = true;
         }
         PanelManager.Open<ResultPanel>(isWin);
     }
     public static void OnMsgLeaveBattle(FrameWorkMsgBase msgBase)
     {
         MsgLeaveBattle msg = (MsgLeaveBattle) msgBase;
         BaseTank tank = GetTank(msg.id);
         if (tank == null)
         {
             return;
         }
         RemoveTank(msg.id);
         MonoBehaviour.Destroy(tank.gameObject);
     }

     public static void GenerateTank(TankInfo tankInfo)
     {
         string objName = "Tank_" + tankInfo.id;
         GameObject tankObj = new GameObject(objName);

         BaseTank tank = null;
         if (tankInfo.id == GameMain.id)
         {
             tank = tankObj.AddComponent<CtrlTank>();
         }
         else
         {
             tank = tankObj.AddComponent<SyncTank>();
         }

         if (tankInfo.id == GameMain.id)
         {
             CameraFollow cf = tankObj.AddComponent<CameraFollow>();
         }

         tank.camp = tankInfo.camp;
         tank.id = tankInfo.id;
         tank.hp = tankInfo.hp;

         Vector3 pos = new Vector3(tankInfo.x,tankInfo.y,tankInfo.z);
         Vector3 rot = new Vector3(tankInfo.ex,tankInfo.ey,tankInfo.ez);

         tank.transform.position = pos;
         tank.transform.eulerAngles = rot;
         if (tankInfo.camp == 1)
         {
             tank.Init("tankPrefab");
         }
         else
         {
             tank.Init("tankPrefab2");
         }
         AddTank(tankInfo.id,tank);
     }

     public static void EnterBattle(MsgEnterBattle msg)
     {
         Reset();
         PanelManager.Close("RoomPanel");
         PanelManager.Close("ResultPanel");
         for (int i = 0; i < msg.tanks.Length; i++)
         {
             GenerateTank(msg.tanks[i]);
         }
     }

     public static void AddTank(string id, BaseTank tank)
     {
         tanks.Add(id,tank);
     }

     public static void RemoveTank(string id)
     {
         tanks.Remove(id);
     }

     public static BaseTank GetTank(string id)
     {
         if (tanks.ContainsKey(id))
         {
             return tanks[id];
         }

         return null;
     }

     public static BaseTank GetCtrlTank()
     {
         return GetTank(GameMain.id);
     }

     public static void Reset()
     {
         foreach (BaseTank tank in tanks.Values)
         {
             MonoBehaviour.Destroy(tank.gameObject);
         }
     }
}
