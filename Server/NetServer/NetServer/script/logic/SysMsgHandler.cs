using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


partial class SysMsgHandler
{
    public static void MsgPing(ClientState c, MsgBase MsgBase)
    {
        Console.WriteLine("msgPing");
        c.lastPingTime = NetManager.GetTimeStamp();
        MsgPong msgPong = new MsgPong();
        NetManager.Send(c,msgPong);
    }
}