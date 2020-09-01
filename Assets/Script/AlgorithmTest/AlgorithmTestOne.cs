using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

public class AlgorithmTestOne : MonoBehaviour {

    //R:向右转
    //L:向左转
    //F:向前走 
    //玩家只能使用上面几种指令来控制角色，指令可以叠加，属于一组指令可以让角色不是在沿着某个半径的圆行走。
    private Dictionary<int,Vector2> posDictionary = new Dictionary<int, Vector2>();


    private int index = 1;

    public int Index
    {
        get { return index; }
        set
        {
            if (value > posDictionary.Count)
            {
                value = 1;
            }
            else if(value < 1)
            {
                value = posDictionary.Count;
            }
            index = value;
        }
    }

    private Vector2 curPos = Vector2.zero;
    private Vector2 curForward = new Vector2(0,1);
	void Start () {
        posDictionary.Add(1, new Vector2(0,1));
        posDictionary.Add(2, new Vector2(1, 0));
        posDictionary.Add(3, new Vector2(0, -1));
        posDictionary.Add(4, new Vector2(-1, 0));
        string str = "FRLFRL";
        Test(str);
    }

    public bool Test(string str)
    {
        char[] chars = str.ToCharArray();
        foreach (var input in chars)
        {
            switch (input)
            {
                case 'R':
                    Index++;
                    break;
                case 'L':
                    Index--;
                    break;
                case 'F':
                    curForward = posDictionary[Index];
                    curPos = new Vector2(curPos.x+curForward.x,curPos.y+curForward.y);
                    break;
                default:
                    break;
            }
        }

        if (curPos == Vector2.zero || (Index ==2 || index == 3))
        {
            Debug.Log("is error");
            return false;
        }
        Debug.Log("is right");
        return true;
    }
    
	
}
