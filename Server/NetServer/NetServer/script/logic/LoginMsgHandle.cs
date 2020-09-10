using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

partial class SysMsgHandler
{
    public static void MsgRegister(ClientState c, MsgBase msgBase)
    {
        MsgRegister msg = (MsgRegister) msgBase;
        if (DbManager.Register(msg.id,msg.pw))
        {
            DbManager.CreatePlayer(msg.id);
            msg.result = 0;
        }
        else
        {
            msg.result = 1;
        }
        NetManager.Send(c,msg);
    }

    public static void MsgLogin(ClientState c, MsgBase msgBase)
    {
        MsgLogin msg = (MsgLogin)msgBase;
        if (DbManager.CheckPassword(msg.id, msg.pw))
        {
            msg.result = 1;
            NetManager.Send(c,msg);
            return;
        }

        if (c.player!=null)
        {
            msg.result = 1;
            NetManager.Send(c,msg);
            return;
        }

        if (PlayerManager.IsOnline(msg.id))
        {
            Player other = PlayerManager.GetPlayer(msg.id);
            MsgKick msgKick = new MsgKick();
            msg.result = 0;
            other.Send(msgKick);
            NetManager.Close(other.state);
        }

        //获取玩家数据
        PlayerData playerData = DbManager.GetPlayerData(msg.id);
        if (playerData == null)
        {
            msg.result = 1;
            NetManager.Send(c,msg);
            return;
        }

        Player player = new Player(c);
        player.id = msg.id;
        player.data = playerData;
        PlayerManager.AddPlayer(msg.id,player);
        c.player = player;
        msg.result = 0;
        player.Send(msg);
    }

    public static void OnDisconnect(ClientState c)
    {
        Console.WriteLine("Close");
        if (c.player != null)
        {
            DbManager.UpdatePlayerData(c.player.id, c.player.data);
            PlayerManager.RemovePlayer(c.player.id);
        }
    }
}
