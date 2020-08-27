using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Net.Sockets;
using NetServer.script.logic;

namespace NetServer.script.net
{
    class ClientState
    {
        public Socket socket;
        public ByteArray readBuff = new ByteArray();
        //心跳时间
        public long lastPingTime = 0;

        //玩家
        public Player player;

    }
}
