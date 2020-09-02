using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine;

public class LengthOfLongestSubstring : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        Debug.Log(Test(" ")); 
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public int Test(string s)
    {
        List<char> tempDic = new List<char>();
        int curMaxLength = 0;
        for (int i = 0; i < s.Length; i++)
        {
            if (tempDic.Contains(s[i]))
            {
                if (curMaxLength < tempDic.Count)
                    curMaxLength = tempDic.Count;
                int sameIndex = 0;
                for (int j = 0; j < tempDic.Count; j++)
                {
                    if (tempDic[j] == s[i])
                    {
                        sameIndex = j;
                    }
                }
                tempDic.RemoveRange(0, sameIndex+1);
                tempDic.Add(s[i]);
            }
            else
            {
                tempDic.Add(s[i]);
            }
        }
        if (curMaxLength < tempDic.Count)
            curMaxLength = tempDic.Count;
        return curMaxLength;
    }
}
