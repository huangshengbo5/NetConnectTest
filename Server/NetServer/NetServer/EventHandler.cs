using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetServer.script.net;

namespace NetServer
{
    class EventHandler
    {
        public static void OnDisConnect(ClientState c)
        {
            Console.WriteLine("OnDisconnect:  "+ c.socket.LocalEndPoint.ToString());
        }
    }
}
