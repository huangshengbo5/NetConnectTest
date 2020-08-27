using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.AccessControl;
using NetServer.script.logic;
using NetServer.script.net;

namespace NetServer
{
    //class ClientState
    //{
    //    public Socket socket;
    //    public  byte [] readBuff = new byte[1024];
    //    public int hp = -100;
    //    public float x = 0;
    //    public float y = 0;
    //    public float z = 0;
    //    public float euly = 0;
    //}

    class Program
    {
        static void Main(string[] args)
        {
            //MainAccess();
            if (!DbManager.Connect("game","127.0.0.1",3306,"root",""))
            {
                return;
            }

            //if (DbManager.Register("lpy","123456"))
            //{
            //    Console.WriteLine("注册成功");
            //}

            //if (DbManager.CreatePlayer("aglab"))
            //{
            //    Console.WriteLine("创角成功");

            //}

            DbManager.CreatePlayer("hsb");
            PlayerData pd = DbManager.GetPlayerData("hsb");
            if (pd != null)
            {
                pd.coin = 100;
                DbManager.UpdatePlayerData("aglab", pd);
            }

            Console.ReadLine();

        }

        //private static Socket listenFd;
        //public static Dictionary<Socket, ClientState> clients = new Dictionary<Socket, ClientState>();

        //public static void MainAccess()
        //{
        //    listenFd = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //    IPAddress ipAdr = IPAddress.Parse("127.0.0.1");
        //    IPEndPoint ipEnd = new IPEndPoint(ipAdr, 9999);
        //    listenFd.Bind(ipEnd);
        //    listenFd.Listen(0);
        //    Console.WriteLine("[Server] is Start");

        //    List<Socket> checkRead = new List<Socket>();
             
        //    while (true)
        //    {
        //        checkRead.Clear();
        //        checkRead.Add(listenFd);
        //        foreach (var VARIABLE in clients.Values)
        //        {
        //            checkRead.Add(VARIABLE.socket);
        //        }

        //        Socket.Select(checkRead, null, null, 1000);
        //        foreach (var VARIABLE in checkRead)
        //        {
        //            if (VARIABLE == listenFd)
        //            {
        //                ReadListenfd(VARIABLE);
        //            }
        //            else
        //            {
        //                ReadClientfd(VARIABLE);
        //            }
        //        }
        //    }
        //}

        //public static void ReadListenfd(Socket s)
        //{
        //    Console.WriteLine("Accept");
        //    Socket clientFd = s.Accept();
        //    ClientState state = new ClientState();
        //    state.socket = clientFd;
        //    clients.Add(clientFd, state);
        //}

        //public static bool ReadClientfd(Socket s)
        //{
        //    ClientState state = clients[s];
        //    int count = 0;
        //    try
        //    {
        //        //count = s.Receive(state.readBuff);
        //    }
        //    catch (SocketException e)
        //    {
        //        MethodInfo mei = typeof(EventHandler).GetMethod("OnDisConnect");
        //        object[] ob = { state };
        //        mei.Invoke(null, ob);

        //        s.Close();
        //        clients.Remove(s);
        //        Console.WriteLine(e);
        //        throw;
        //    }

        //    if (count == 0)
        //    {
        //        MethodInfo mei = typeof(EventHandler).GetMethod("OnDisConnect");
        //        object[] ob = { state };
        //        mei.Invoke(null, ob);
        //        s.Close();
        //        clients.Remove(s);
        //    }

        //    string receStr = System.Text.Encoding.Default.GetString(state.readBuff, 0, count);

        //    //add
        //    string[] splitStrs = receStr.Split('|');
        //    //add
        //    Console.WriteLine("[Server] Receive:     " + receStr);

        //    string msgName = splitStrs[0];
        //    string msgArgs = splitStrs[1];
        //    string funcName = "Msg" + msgName;
        //    MethodInfo mi = typeof(MsgHandler).GetMethod(funcName);
        //    object[] o = { state,msgArgs };
        //    mi.Invoke(null, o);
        //    return true;

        //    //byte[] sendBytes = System.Text.Encoding.Default.GetBytes(receStr);
        //    //foreach (var VARIABLE in clients.Keys)
        //    //{
        //    //    VARIABLE.Send(sendBytes);
        //    //}
        //}

        //public static void Send(ClientState state ,string str)
        //{
        //    byte[] sendMsg = System.Text.Encoding.Default.GetBytes(str);
        //    state.socket.Send(sendMsg);
        //}
    }
}
