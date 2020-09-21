using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class Player
    {
        public string id = "";
        public ClientState state;
        //坐标和旋转
        public float x;
        public float y;
        public float z;
        public float ex;
        public float ey;
        public float ez;
        public float turretY;
        //房间号
        public int roomId = -1;
        //阵营
        public int camp = 1;
        //坦克生命值
        public int hp = 100;


        //数据库数据
        public PlayerData data;

        public Player(ClientState state)
        {
            this.state = state;
        }

        public void Send(MsgBase msgBase)
        {
            NetManager.Send(state,msgBase);
        }
    }

