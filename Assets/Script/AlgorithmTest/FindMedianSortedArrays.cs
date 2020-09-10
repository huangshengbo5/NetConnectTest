using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class FindMedianSortedArrays : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        int[] one = {1,2,4};
        int[] two = {1};
        Debug.Log(test(one,two)); 
    }

    public float test(int[] listOne, int[] listTwo)
    {
        int one1 = -1;
        int one2 = -1;
        int two1 = -1;
        int two2 = -1;
        int oneLength = listOne.Length;
        int twoLength = listTwo.Length;
        if (listOne.Length > 0)
        {
             one1 = listOne[0];
             if (listOne.Length>1)
                 one2 = listOne[listOne.Length - 1];
        }

        if (listTwo.Length>=2)
        {
            two1 = listTwo[0];
            two2 = listTwo[listTwo.Length - 1];
        }
        else if (listTwo.Length == 1)
        {
            two2 = listTwo[0];
        }

        if (one1 >0 && two1 >0)
        {
            if (one1 > two1 )
            {
                one1 = two1;
            }
        }
        else if (one1<0 && two1>=0)
        {
            one1 = two1;
        }

        one1 = one1 == -1 ? 0 : one1;
        if (one2>0 && two2>0)
        {
            if (one2 < two2)
            {
                one2 = two2;
            }
        }
        else if (one2 <0 && two2 >=0)
        {
            one2 = two2;
        }

        one2 = one2 == -1 ? 0 : one2;
        int devNum = 2;
        if (oneLength + twoLength==1)
        {
            devNum = 1;
        }
        return (float)(one1 + one2) / devNum;
    }


}
