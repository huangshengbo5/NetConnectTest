using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MsgSyncTank : MsgBase
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

public class MsgFire : MsgBase
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

public class MsgHit : MsgBase
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
