using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlTank : BaseTank {

	// Update is called once per frame
	new void Update ()
    {
        base.Update();
        MoveUpdate();
        TurrentUpdate();
        FireUpdate();
    }

    public void MoveUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        transform.Rotate(0,x*steer*Time.deltaTime,0);

        float y = Input.GetAxis("Vertical");
        Vector3 s = y * transform.forward * speed * Time.deltaTime;

        transform.position += s;
    }

    //旋转炮台
    public void TurrentUpdate()
    {
        float axis = 0;
        if (Input.GetKey(KeyCode.Q))
        {
            axis = -1;
        }
        else if (Input.GetKey((KeyCode.K)))
        {
            axis = 1;
        }
        //旋转角度
        Vector3 le = turret.localEulerAngles;
        le.y += axis * Time.deltaTime * turretSpeed;
        turret.localEulerAngles = le;
    }

    public void FireUpdate()
    {
        if (!Input.GetKey(KeyCode.Space))
        {
            return;
        }

        if (Time.time - lastFireTime < firCd)
        {
            return;
        }
        Fire();
    }
}
