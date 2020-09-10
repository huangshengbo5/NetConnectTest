using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;


partial class EventHandler
{
    public static void OnDisconnect(ClientState c)
    {
        Console.WriteLine("Close");
        if (c.player != null)
        {
            int roomId = c.player.roomId;
            if (roomId >=0)
            {
                Room room = RoomManager.GetRoom(roomId);
                room.RemovePlayer(c.player.id);
            }
            //保存数据
            DbManager.UpdatePlayerData(c.player.id, c.player.data);
            PlayerManager.RemovePlayer(c.player.id);
        }
    }

    public static void OnTimer()
    {
        CheckPing();
    }

    public static void CheckPing()
    {
        long timeNow = NetManager.GetTimeStamp();
        foreach (ClientState s in NetManager.clients.Values)
        {
            if (timeNow-s.lastPingTime>NetManager.pingInterval*4)
            {
                Console.WriteLine("Ping Close" + s.socket.RemoteEndPoint.ToString());
                NetManager.Close(s);
                return;
            }
        }
    }
}   
