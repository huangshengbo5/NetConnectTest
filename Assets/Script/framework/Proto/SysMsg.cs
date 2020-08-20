using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgPing : FrameWorkMsgBase {
    public MsgPing()
    {
        protoName = "MsgPing";
    }

}

public class MsgPong : FrameWorkMsgBase
{
    public MsgPong()
    {
        protoName = "MsgPong";
    }
}