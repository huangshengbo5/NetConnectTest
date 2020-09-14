﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//查询战绩
public class MsgGetAchieve : FrameWorkMsgBase {

    public MsgGetAchieve()
    {
        protoName = "MsgGetAchieve";
    }

    public int win = 0;
    public int lost = 0;
}


//房间信息
public class RoomInfo
{
    public int id = 0; //房间id
    public int count = 0; //人数
    public int status = 0; //状态 0：准备中 1 战斗中
}

//查询房间列表
public class MsgGetRoomList : FrameWorkMsgBase
{
    public MsgGetRoomList()
    {
        protoName = "MsgGetRoomList";
    }

    public RoomInfo[] rooms;
}

//创建房间
public class MsgCreateRoom : FrameWorkMsgBase
{
    public MsgCreateRoom()
    {
        protoName = "MsgCreateRoom";
    }

    public int result = 0;
}

//进入房间
public class MsgEnterRoom : FrameWorkMsgBase
{
    public MsgEnterRoom()
    {
        protoName = "MsgEnterRoom";
    }
    //客户端发
    public int id = 0;
    //服务器返回
    public int result = 0;
}

//房间信息
public class PlayerInfo
{
    public string id = "lpy";//账号
    public int camp = 0;     //阵营
    public int win = 0;      //胜利数
    public int lost = 0;     //失败数
    public int isOwner = 0;  //是否是房主
}

//查询房间信息
public class MsgGetRoomInfo : FrameWorkMsgBase
{
    public MsgGetRoomInfo()
    {
        protoName = "MsgGetRoomInfo";
    }

    public PlayerInfo[] players;
}

//退出房间
public class MsgLeaveRoom : FrameWorkMsgBase
{
    public MsgLeaveRoom()
    {
        protoName = "MsgLeaveRoom";
    }

    public int result = 0;
}

//开始战斗
public class MsgStartBattle : FrameWorkMsgBase
{
    public MsgStartBattle()
    {
        protoName = "MsgStartBattle";
    }

    public int result = 0;
}


public class TankInfo
{
    public string id = "";
    public int camp = 0;
    public int hp = 0;
    public float x = 0;
    public float y = 0;
    public float z = 0;
    public float ex = 0;
    public float ey = 0;
    public float ez = 0;
}
public class MsgEnterBattle : FrameWorkMsgBase
{
    public MsgEnterBattle()
    {
        protoName = "MsgEnterBattle";
    }

    public TankInfo[] tanks;
    public int mapId = 1;
}

public class MsgBattleResult : FrameWorkMsgBase
{
    public MsgBattleResult()
    {
        protoName = "MsgBattleResult";
    }

    public int winCamp = 0;
}

public class MsgLeaveBattle : FrameWorkMsgBase
{
    public MsgLeaveBattle()
    {
        protoName = "MsgLeaveBattle";
    }

    public string id = "";
}