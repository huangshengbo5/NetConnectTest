using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncTank : BaseTank
{
    private Vector3 lastPos;
    private Vector3 lastRot;
    private Vector3 forecastPos;
    private Vector3 forecastRot;
    private float forecastTime;


    public override void Init(string skinPath)
    {
        base.Init(skinPath);
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        rigidbody.useGravity = false;
        lastPos = transform.position;
        lastRot = transform.eulerAngles;
        forecastPos = transform.position;
        forecastRot = transform.eulerAngles;
        forecastTime = Time.time;
    }

    public void SyncPos(MsgSyncTank msg)
    {
        Vector3 pos = new Vector3(msg.x,msg.y,msg.z);
        Vector3 rot = new Vector3(msg.ex,msg.ey,msg.ez);
        forecastPos = pos + 2 * (pos - lastPos);
        forecastRot = rot + 2 * (rot - lastRot);

        lastPos = pos;
        lastRot = rot;
        forecastTime = Time.time;
        Vector3 le = turret.localEulerAngles;
        le.y = msg.turretY;
        turret.localEulerAngles = le;
    }

    public void SyncFire(MsgFire msg)
    {
        TankBullet bullet = Fire();
        Vector3 pos = new Vector3( msg.x,msg.y,msg.z);
        Vector3 rot = new Vector3(msg.ex,msg.ey,msg.ez);
        bullet.transform.position = pos;
        bullet.transform.eulerAngles = rot;
    }
    new void Update()
    {
        base.Update();
        ForecastUpdate();
    }

    public void ForecastUpdate()
    {
        float t = (Time.time - forecastTime) / CtrlTank.syncInterval;
        t = Mathf.Clamp(t, 0, 1f);
        Vector3 pos = transform.position;
        pos = Vector3.Lerp(pos, forecastPos, t);
        transform.position = pos;

        Quaternion quat = transform.rotation;
        Quaternion forecastQuat = Quaternion.Euler(forecastRot);
        quat = Quaternion.Lerp(quat, forecastQuat, t);
        transform.rotation = quat;
    }
}

