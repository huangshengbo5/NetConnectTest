using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetServer
{
    class MsgHandler
    {
        public static void MsgEnter(ClientState c, string msgArgs)
        {
            Console.WriteLine("MsgEnter:  "+ msgArgs);
            string[] split = msgArgs.Split(',');
            string desc = split[0];
            float x = float.Parse(split[1]);
            float y = float.Parse(split[1]);
            float z = float.Parse(split[1]);
            float rulY = float.Parse(split[1]);
            c.x = x;
            c.y = y;
            c.z = z;
            c.euly = rulY;
            string sendStr = "Enter|" + msgArgs;
            foreach (var VARIABLE in Program.clients.Values)
            {
                Program.Send(VARIABLE,sendStr);
            }
        }

        public static void MsgList(ClientState c, string Args)
        {
            Console.WriteLine("MsgList:  " + Args);
            string sendStr = "List|";
            foreach (var VARIABLE in Program.clients.Values)
            {
                sendStr += VARIABLE.socket.RemoteEndPoint.ToString() + ",";
                sendStr += VARIABLE.x.ToString();
                sendStr += VARIABLE.y.ToString();
                sendStr += VARIABLE.z.ToString();
                sendStr += VARIABLE.euly.ToString();
                sendStr += VARIABLE.hp.ToString();
            }
            Program.Send(c,sendStr);
        }
    }
}
