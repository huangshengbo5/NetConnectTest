using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class MsgGetText : MsgBase
{
    public MsgGetText()
    {
        protoName = "MsgGetText";
    }

    public string text = "";
}

class MsgSaveText : MsgBase
{
    public MsgSaveText()
    {
        protoName = "MsgSaveText";
    }
    public string text = "";
    public int result = 0;
}


