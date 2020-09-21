using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

partial class SysMsgHandler
{
    public static void MsgStartBattle(ClientState c, MsgBase msgBase)
    {
        MsgStartBattle msg = (MsgStartBattle) msgBase;
        Player player = c.player;
        if (player ==null)
        {
            return;
        }

        Room room = RoomManager.GetRoom(player.roomId);
        if (room == null)
        {
            msg.result = 1;
            player.Send(msg);
            return;
        }

        if (!room.isOwner(player))
        {
            msg.result = 1;
            player.Send(msg);
            return;
        }

        if (!room.StartBattle())
        {
            msg.result = 1;
            player.Send(msg);
            return;
        }

        msg.result = 0;
        player.Send(msg);
    }
}
    
