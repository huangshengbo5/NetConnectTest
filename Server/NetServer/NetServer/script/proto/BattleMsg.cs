using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;

class BattleMsg
{

    public class TankInfo
    {
        public string id = "";
        public int camp = 0;
        public int hp = 0;
        public float x = 0;
        public float y = 0;
        public float z = 0;
        public float ex = 0;
        public float ey = 0;
        public float ez = 0;
    }
    public class MsgEnterBattle : MsgBase
    {
        public MsgEnterBattle()
        {
            protoName = "MsgEnterBattle";
        }

        public TankInfo[] tanks;
        public int mapId = 1;
    }

    public class MsgBattleResult :MsgBase
    {
        public MsgBattleResult()
        {
            protoName = "MsgBattleResult";
        }

        public int winCamp = 0;
    }

    public class MsgLeaveBattle : MsgBase
    {
        public MsgLeaveBattle()
        {
            protoName = "MsgLeaveBattle";
        }

        public string id = "";
    }

}