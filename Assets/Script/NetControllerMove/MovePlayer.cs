using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MoveBase
{

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
            }
        }
    }
}
