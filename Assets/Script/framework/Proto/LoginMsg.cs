﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor.MemoryProfiler;


class MsgLogin : FrameWorkMsgBase
{
    public MsgLogin()
    {
        protoName = "MsgLogin";
    }
    public string id;
    public string pw;
    public int result;
}

class MsgRegister : FrameWorkMsgBase
{
    public MsgRegister()
    {
        protoName = "MsgRegister";
    }

    public string id;
    public string pw;
    public int result;
}
