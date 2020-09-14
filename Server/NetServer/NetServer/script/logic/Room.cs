using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;


class Room
{
    public int id = 0;
    public int maxPlayer = 6;
    public Dictionary<string, bool> playerIds = new Dictionary<string, bool>();
    public string ownerId = "";

    public enum Status
    {
        PREPARE = 0,
        FIGHT = 1,
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
        if (status == Status.FIGHT)
        {
            player.data.lost++;
            BattleMsg.MsgLeaveBattle msg = new BattleMsg.MsgLeaveBattle();
            msg.id = player.id;
            Broadcast(msg);
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

            if (player.camp == 2)
            {
                count2++;
            }
        }

        return count1 >= count2 ? 2 : 1;
    }

    public bool CanStartBattle()
    {
        if (status != Status.PREPARE)
        {
            return false;
        }

        int count1 = 0;
        int count2 = 0;
        foreach (string id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            if (player.camp == 1)
            {
                count1++;
            }
            else
            {
                count2++;
            }
        }

        if (count1 < 1 || count2 < 1)
        {
            return false;
        }

        return true;
    }

    static float[,,] birthConfig = new float[2, 3, 6]
    {
        {
            {-85f, 3.8f, -33.8f, 0, 24.9f, 0f},
            {-49f, 3.8f, -61.4f, 0f, 21.4f, 0f},
            {-6.2f, 3.8f, -70f, 0, 21.9f, 0},
        },
        {
            {-150f, 0f, 179f, 0, -156f, 0f},
            {-105f, 0f, 216f, 0f, -156f, 0f},
            {-52f, 0f, -239f, 0, -156f, 0},
        },
    };

    private void SetBrithPos(Player player, int index)
    {
        int camp = player.camp;
        player.x = (int)birthConfig[camp - 1, index, 0];
        player.y = (int)birthConfig[camp - 1, index, 1];
        player.z = (int)birthConfig[camp - 1, index, 2];
        player.ex = birthConfig[camp - 1, index, 3];
        player.ey = birthConfig[camp - 1, index, 4];
        player.ez = birthConfig[camp - 1, index, 5];
    }

    private void ResetPlayers()
    {
        int count1 = 0;
        int count2 = 0;
        foreach (string  id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            if (player.camp == 1)
            {
                SetBrithPos(player,count1);
                count1++;
            }
            else
            {
                SetBrithPos(player, count2);
                count2++;
            }
        }

        foreach (string id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            player.hp = 100;
        }
    }

    public BattleMsg.TankInfo PlayerToTankInfo(Player player)
    {
        BattleMsg.TankInfo tankInfo = new BattleMsg.TankInfo();
        tankInfo.camp = player.camp;
        tankInfo.id = player.id;
        tankInfo.hp = player.hp;
        tankInfo.x = player.x;
        tankInfo.y = player.y;
        tankInfo.z = player.z;
        tankInfo.ex = player.ex;
        tankInfo.ey = player.ey;
        tankInfo.ez = player.ez;
        return tankInfo;
    }

    public bool StartBattle()
    {
        if (!CanStartBattle())
        {
            return false;
        }

        status = Status.FIGHT;

        ResetPlayers();
        BattleMsg.MsgEnterBattle msg = new BattleMsg.MsgEnterBattle();
        msg.mapId = 1;
        msg.tanks = new BattleMsg.TankInfo[playerIds.Count];

        int i = 0;
        foreach (string  id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            msg.tanks[i] = PlayerToTankInfo(player);
            i++;
        }
        Broadcast(msg);
        return true;
    }

    public bool IsDie(Player player)
    {
        return player.hp <= 0;
    }

    public int Judgment()
    {
        int count1 = 0;
        int count2 = 0;
        foreach (string  id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            if (!IsDie(player))
            {
                if (player.camp ==1)
                {
                    count1++;
                }

                if (player.camp == 2)
                {
                    count2++;
                }
            }
        }

        if (count1 <=0)
        {
            return 2;

        }
        else if (count2<=0)
        {
            return 1;
        }

        return 0;
    }


    private long lastJudgeTime = 0;
    public  void Update()
    {
        if (status != Status.FIGHT)
        {
            return;
        }

        if (NetManager.GetTimeStamp() - lastJudgeTime >10f)
        {
            return;
        }

        lastJudgeTime = NetManager.GetTimeStamp();
        int winCamp = Judgment();
        if (winCamp == 0)
        {
            return;
        }

        status = Status.PREPARE;
        foreach (string id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            if (player.camp == winCamp)
            {
                player.data.win++;
            }
            else
            {
                player.data.lost++;
            }
        }

        BattleMsg.MsgBattleResult msg = new BattleMsg.MsgBattleResult();
        msg.winCamp = winCamp;
        Broadcast(msg);
    }
}

