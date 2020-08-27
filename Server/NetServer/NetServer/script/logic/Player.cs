using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetServer.script.net;

namespace NetServer.script.logic
{
    class Player
    {
        public string id = "";
        public ClientState state;
        public int x;
        public int y;
        public int z;

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
}
