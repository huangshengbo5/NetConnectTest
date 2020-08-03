using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public enum NetType
{
    Init = 0,
    EnterSence = 1,
    Move = 2,
    LeaveSence = 3,
}

public static class Uitil  {

  

    //获取本地的ip地址
    public static string GetHostIPAdress()
    {
        string hostName = "";
        foreach (var item in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
        {
            if (item.AddressFamily.ToString() == "InterNetwork")
            {
                hostName = item.ToString();
            }
        }
        return hostName;
    }
	
}
