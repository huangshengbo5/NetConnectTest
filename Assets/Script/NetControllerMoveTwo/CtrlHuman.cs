using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlHuman : BaseHuman {

	// Use this for initialization
	new void Start () {
		base.Start();
	}
	
	// Update is called once per frame
    new void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            if (hit.collider.tag == "Terrain")
            {
                MoveTo(hit.point);
                string sendStr = "Move|";
                sendStr += NetManager.GetDesc() + ",";
                sendStr += hit.point.x + ",";
                sendStr += hit.point.y + ",";
                sendStr += hit.point.z + ",";
                NetManager.Send(sendStr);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (isAttacking) return;
            if (isMoveing) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            transform.LookAt(hit.point);
            Attack();
            string sendStr = "Attack|";
            sendStr += NetManager.GetDesc() + ",";
            sendStr += transform.eulerAngles.y + ",";
            NetManager.Send(sendStr);

            //攻击判断
            Vector3 lineEnd = transform.position + 0.5f * Vector3.up;
            Vector3 lineStart = lineEnd + 20 * transform.forward;
            if (Physics.Linecast(lineStart,lineEnd,out hit))
            {
                GameObject hitObj = hit.collider.gameObject;
                if (hitObj == gameObject)
                {
                    return;
                }

                SyncHuman syncHuman = hitObj.GetComponent<SyncHuman>();
                if (syncHuman == null)
                {
                    return;
                }

                string sendString = "Hit|";
                sendString += NetManager.GetDesc() + ",";
                sendString += syncHuman.desc + ",";
                NetManager.Send(sendString);
            }
        }
    }
}
