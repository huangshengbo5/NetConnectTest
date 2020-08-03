using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MoveBase
{
    private void Start()
    {
        MoveNetManager.GetInstance().StartConnect();
    }

    new void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            if (hit.collider.tag == "Land")
            {
                SetTarget(hit.point);
                SendPostion(hit.point);
            }
        }
    }


    public void SendPostion(Vector3 pos)
    {
        int type = (int) NetType.Move;
        string posStr = pos.x.ToString() + '.' + pos.y.ToString() + '.' + pos.z.ToString();
        string message = type.ToString() + "," + posStr;
        MoveNetManager .GetInstance().SendMessage(message);
    }
}
