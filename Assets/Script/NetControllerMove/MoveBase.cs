using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class MoveBase : MonoBehaviour
{
    private bool isCanMove = false;

    private float fSpeed = 1.0f;

    private Vector3 vecTarget;

    public virtual void MoveTo()
    {
        Vector3 myPos = this.transform.position;
        Vector3 myForward = transform.up;
        if (Vector3.Distance(myPos, vecTarget) > 0.1f)
        {
            Vector3 value =  Vector3.Normalize(vecTarget-myPos);
           transform.position =myPos + value * fSpeed * Time.deltaTime;
        }

    }

    public void SetTarget(Vector3 pos)
    {
        Vector3 myPos = this.transform.position;
        Vector3 newPos = new Vector3(pos.x,this.transform.position.y,pos.z);
        vecTarget = newPos;
        isCanMove = Vector3.Distance(myPos, newPos) > 0.1f;
    }

   	public void Update () {
        if (!isCanMove)
        {
            return;
        }
        MoveTo();
    }
}
