using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetServer.script.net;


namespace NetServer
{
    //class MsgHandler
    //{
    //    public static void MsgEnter(ClientState c, string msgArgs)
    //    {
    //        Console.WriteLine("MsgEnter:  "+ msgArgs);
    //        string[] split = msgArgs.Split(',');
    //        string desc = split[0];
    //        float x = float.Parse(split[1]);
    //        float y = float.Parse(split[1]);
    //        float z = float.Parse(split[1]);
    //        float rulY = float.Parse(split[1]);
    //        c.x = x;
    //        c.y = y;
    //        c.z = z;
    //        c.euly = rulY;
    //        string sendStr = "Enter|" + msgArgs;
    //        foreach (var VARIABLE in Program.clients.Values)
    //        {
    //            Program.Send(VARIABLE,sendStr);
    //        }
    //    }

    //    public static void MsgList(ClientState c, string Args)
    //    {
    //        Console.WriteLine("MsgList:  " + Args);
    //        string sendStr = "List|";
    //        foreach (var VARIABLE in Program.clients.Values)
    //        {
    //            sendStr += VARIABLE.socket.RemoteEndPoint.ToString() + ",";
    //            sendStr += VARIABLE.x.ToString() + ",";
    //            sendStr += VARIABLE.y.ToString() + ",";
    //            sendStr += VARIABLE.z.ToString() + ",";
    //            sendStr += VARIABLE.euly.ToString() + ",";
    //            sendStr += VARIABLE.hp.ToString() + ",";
    //        }

    //        Console.WriteLine("[Server] send:  "+ sendStr);
    //        foreach (ClientState cs in Program.clients.Values)
    //        {
    //            Program.Send(cs, sendStr);
    //        }
    //    }
    //    public static void MsgMove(ClientState c, string Args)
    //    {
    //        Console.WriteLine("MsgMove:  " + Args);
    //        string sendStr = "Move|";
    //        string [] split = Args.Split(',');
    //        string desc = split[0];
    //        float x = float.Parse(split[1]);
    //        float y = float.Parse(split[2]);
    //        float z = float.Parse(split[3]);
    //        c.x = x;
    //        c.y = y;
    //        c.z = z;
    //        sendStr += Args;
    //        foreach (ClientState state in Program.clients.Values)
    //        {
    //            Program.Send(state,sendStr);
    //        }
    //    }

    //    public static void OnDisConnect(ClientState c, string Args)
    //    {
    //        Console.WriteLine("MsgDisConnect:  " + Args);
    //        string[] split = Args.Split(',');
    //        string desc = split[0];
    //        string sendStr = "Leave|";
    //        sendStr += desc;
    //        foreach (ClientState state in Program.clients.Values)
    //        {
    //            Program.Send(state,sendStr);
    //        }
    //    }

    //    public static void MsgAttack(ClientState c, string Args)
    //    {
    //        string sendStr = "Attack|" + Args;
    //        foreach (ClientState state in Program.clients.Values)
    //        {
    //            Program.Send(state,sendStr);
    //        }
    //    }

    //    public static void MsgHit(ClientState c, string Args)
    //    {
    //        string[] split = Args.Split(',');
    //        string attDesc = split[0];
    //        string hitDesc = split[1];
    //        ClientState hitCS = null;
    //        foreach (ClientState cs in Program.clients.Values)
    //        {
    //            if (cs.socket.RemoteEndPoint.ToString() == hitDesc)
    //            {
    //                hitCS = cs;
    //            }
    //        }

    //        if (hitCS == null)
    //        {
    //            return;
    //        }

    //        hitCS.hp -= 25;
    //        if (hitCS.hp<=0)
    //        {
    //            string sendStr = "Die|" + hitCS.socket.RemoteEndPoint.ToString();
    //            foreach (ClientState cs in Program.clients.Values)
    //            {
    //                Program.Send(cs,sendStr);
    //            }
    //        }
    //    }
    //}
}
