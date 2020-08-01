using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNetPlayer : MoveBase {

	public void SetNetPostion(Vector3 pos)
    {
        SetTarget(pos);
    }
}
