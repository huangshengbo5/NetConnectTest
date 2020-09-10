using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;


class Room
{
    public int id = 0;
    public int maxPlayer = 6;
    public Dictionary<string ,bool> playerIds = new Dictionary<string, bool>();
    public string ownerId = "";
    public enum Status
    {
       PREPARE = 0,
       FIGHT=1,
    }

    public Status status = Status.PREPARE;

    public bool AddPlayer(string id)
    {
        //获取玩家
        Player player = PlayerManager.GetPlayer(id);
        if (player == null)
        {
            Console.WriteLine("room.AddPlayer fail , Player is null");
            return false;
        }

        if (playerIds.Count >= maxPlayer)
        {
            Console.WriteLine("room.AddPlayer fail , reach maxPlayer");
            return false;
        }

        if (status != Status.PREPARE)
        {
            Console.WriteLine("room.AddPlayer fail, not prepare");
            return false;
        }

        if (playerIds.ContainsKey(id))
        {
            Console.WriteLine("room.AddPlayer fail, already in this room");
            return false;
        }

        playerIds[id] = true;
        player.camp = SwitchCamp();
        player.roomId = this.id;
        if (ownerId == "")
        {
            ownerId = player.id;
        }

        //todo 
        Broadcast(ToMsg());
        return true;
    }

    public bool RemovePlayer(string id)
    {
        Player player = PlayerManager.GetPlayer(id);
        if (player == null)
        {
            Console.WriteLine("room.RemovePlayer fail , player is null");
            return false;
        }

        if (!playerIds.ContainsKey(id))
        {
            Console.WriteLine("room.RemovePlayer fail,not in this room");
            return false;
        }


        playerIds.Remove(id);

        player.camp = 0;
        player.roomId = -1;
        if (isOwner(player))
        {
            ownerId = SwitchOwner();
        }

        if (playerIds.Count == 0)
        {
            RoomManager.RemoveRoom(this.id);
        }
        Broadcast(ToMsg());
        return true;
    }

    public bool isOwner(Player player)
    {
        return player.id == ownerId;
    }

    public string SwitchOwner()
    {
        foreach (var key in playerIds.Keys)
        {
            return key;
        }
        return "";
    }

    //广播消息
    public void Broadcast(MsgBase msgBase)
    {
        foreach (string id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            player.Send(msgBase);
        }
    }

    public MsgBase ToMsg()
    {
        MsgGetRoomInfo msg = new MsgGetRoomInfo();
        int count = playerIds.Count;
        msg.players = new PlayerInfo[count];
        int i = 0;

        foreach (string id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            PlayerInfo playerInfo = new PlayerInfo();
            playerInfo.id = player.id;
            playerInfo.camp = player.camp;
            playerInfo.win = player.data.win;
            playerInfo.lost = player.data.lost;
            playerInfo.isOwner = 0;
            if (isOwner(player))
            {
                playerInfo.isOwner = 1;
            }

            msg.players[i] = playerInfo;
            i++;
        }
        return msg;
    }

    

    //选择一个阵营
    public int SwitchCamp()
    {
        int count1 = 0;
        int count2 = 0;
        foreach (var key in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(key);
            if (player.camp == 1)
            {
                count1++;
            }

            if (player.camp ==2)
            {
                count2++;
            }
        }
        return count1 >= count2 ? 2 : 1;
    }
}

