using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

    //R:向右转
    //L:向左转
    //F:向前走 

    private Dictionary<int,Vector2> posDictionary = new Dictionary<int, Vector2>();


    private int index;
	// Use this for initialization
	void Start () {
        posDictionary.Add(1, new Vector2(0,1));
        posDictionary.Add(2, new Vector2(1, 0));
        posDictionary.Add(3, new Vector2(0, -1));
        posDictionary.Add(4, new Vector2(-1, 0));
	}
	
	
}
