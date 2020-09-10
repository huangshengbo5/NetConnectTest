using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;


partial class SysMsgHandler
{
    //查询战绩
    public static void MsgGetAchieve(ClientState c, MsgBase msgBase)
    {
        MsgGetAchieve msg = (MsgGetAchieve) msgBase;
        Player player = c.player;
        if (player == null)
        {
            return;
        }
        msg.win = player.data.win;
        msg.lost = player.data.lost;
        player.Send(msg);
    }

    public static void MsgGetRoomList(ClientState c, MsgBase msgBase)
    {
        MsgGetRoomList msg = (MsgGetRoomList)msgBase;
        Player player = c.player;
        if (player == null)
        {
            return;
        }
        player.Send(RoomManager.ToMsg());
    }

    //创建房间
    public static void MsgCreateRoom(ClientState c, MsgBase msgBase)
    {
        MsgCreateRoom msg = (MsgCreateRoom)msgBase;
        Player player = c.player;
        if (player == null)
        {
            return;
        }

        if (player.roomId >=0)
        {
            msg.result = 1;
            player.Send(msg);
            return;
        }
        Room room = RoomManager.AddRoom();
        room.AddPlayer(player.id);
        msg.result = 0;
        player.Send(msg);
    }

    public static void MsgEnterRoom(ClientState c, MsgBase msgBase)
    {
        MsgEnterRoom msg = (MsgEnterRoom)msgBase;
        Player player = c.player;
        if (player == null)
        {
            return;
        }

        if (player.roomId >= 0)
        {
            msg.result = 1;
            player.Send(msg);
            return;
        }

        Room room = RoomManager.GetRoom(msg.id);
        if (room==null)
        {
            msg.result = 1; 
            player.Send(msg);
            return;
        }

        if (!room.AddPlayer(player.id))
        {
            msg.result = 1;
            player.Send(msg);
            return;
        }
        msg.result = 0;
        player.Send(msg);
    }

    public static void MsgGetRoomInfo(ClientState c, MsgBase msgBase)
    {
        MsgGetRoomInfo msg = (MsgGetRoomInfo) msgBase;
        Player player = c.player;
        if (player==null)
        {
            return;
        }

        Room room = RoomManager.GetRoom(player.roomId);
        if (room==null)
        {
            player.Send(msg);   
            return;
        }
        player.Send(room.ToMsg());
    }

    public static void MsgLeaveRoom(ClientState c, MsgBase msgBase)
    {
        MsgLeaveRoom msg = (MsgLeaveRoom) msgBase;
        Player player = c.player;
        if (player==null)
        {
            return;
        }

        Room room = RoomManager.GetRoom(player.roomId);
        if (room==null)
        {
            msg.result = 1;
            player.Send(msg);
            return;
        }

        room.RemovePlayer(player.id);
        msg.result = 0;
        player.Send(msg);
    }
}   
