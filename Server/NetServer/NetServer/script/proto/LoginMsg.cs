using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class MsgRegister:MsgBase
{
    public MsgRegister()
    {
        protoName = "MsgRegister";
    }
    public string id = "";
    public string pw = "";
    public int result = 0;
}

class MsgLogin : MsgBase
{
    public MsgLogin()
    {
        protoName = "MsgLogin";
    }
    public string id = "";
    public string pw = "";
    public int result = 0;
}

class MsgKick : MsgBase
{
    public MsgKick()
    {
        protoName = "MsgKick";
    }
    public string id = "";
    public string pw = "";
    public int result = 0;
}
