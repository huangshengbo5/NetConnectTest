using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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
