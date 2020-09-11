using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Newtonsoft.Json;

[Serializable]
public class FrameWorkMsgBase
{
    public string protoName = "";

    //编译为bytes
    public static byte[] Encode(FrameWorkMsgBase msgBase)
    {
        //string s =  JsonUtility.ToJson(msgBase);
        string s =  JsonConvert.SerializeObject(msgBase);
        return System.Text.Encoding.UTF8.GetBytes(s);
    }

    //反编译为 FrameWorkMsgBase
    public static FrameWorkMsgBase Decode(string protoName, byte[] bytes, int offset, int count)
    {
        string s = System.Text.Encoding.UTF8.GetString(bytes, offset, count);
        //FrameWorkMsgBase msgBase = (FrameWorkMsgBase) JsonUtility.FromJson(s, Type.GetType(protoName));
        FrameWorkMsgBase msgBase = (FrameWorkMsgBase)JsonConvert.DeserializeObject(s, Type.GetType(protoName));
        return msgBase;
    }

    //编码协议名（2字节长度+字符串）
    public static byte[] EncodeName(FrameWorkMsgBase msgBase)
    {
        byte[] nameBytes = System.Text.Encoding.UTF8.GetBytes(msgBase.protoName);
        Int16 len = (Int16) nameBytes.Length;

        byte[] bytes = new byte[2 + len];
        bytes[0] = (byte) (len % 256);
        bytes[1] = (byte)(len / 256);

        Array.Copy(nameBytes, 0, bytes, 2, len);
        return bytes;
    }

    //解码协议名（2字节长度+ 字符串）
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bytes">需要解析的信息</param>
    /// <param name="offset">读取协议名称的 index 值</param>
    /// <param name="count">协议名称占用的字节长度</param>
    /// <returns></returns>
    public static string DecodeName(byte[] bytes, int offset, out int count)
    {
        count = 0;
        //必须大于2字节
        if (offset +2 >bytes.Length)
        {
            return "";
        }

        //读取名字长度
        Int16 len = (Int16) (bytes[offset + 1] << 8 | bytes[offset]);
        if (len<=0)
        {
            return "";
        }

        if (offset+2+len >bytes.Length)
        {
            return "";
        }

        count = 2 + len;
        string name = System.Text.Encoding.UTF8.GetString(bytes, offset + 2, len);
        return name;
    }

}
