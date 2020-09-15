using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MsgSyncTank : FrameWorkMsgBase
{
    public MsgSyncTank()
    {
        protoName = "MsgSyncTank";
    }

    public float x = 0f;
    public float y = 0f;
    public float z = 0f;
    public float ex = 0f;
    public float ey = 0f;
    public float ez = 0f;
    public float turretY = 0f;
    public string id = "";
}

public class MsgFire : FrameWorkMsgBase
{
    public MsgFire()
    {
        protoName = "MsgFire";
    }

    public float x = 0f;
    public float y = 0f;
    public float z = 0f;
    public float ex = 0f;
    public float ey = 0f;
    public float ez = 0f;
    public string id = "";
}

public class MsgHit : FrameWorkMsgBase
{
    public MsgHit()
    {
        protoName = "MsgHit";
    }

    public string targetId = "";
    public float x = 0f;
    public float y = 0f;
    public float z = 0f;
    public string id = "";
    public int hp = 0;
    public int damage = 0;
}
