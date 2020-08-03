using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Security.AccessControl;

namespace NetServer
{
    class ClientState
    {
        public Socket socket;
        public byte[] readBuff = new Byte [1024];
    }
    class Program
    {
        static void Main(string[] args)
        {
            AsyncServer();
        }



        public static Dictionary<Socket, ClientState> ClientStates = new Dictionary<Socket, ClientState>();
        //异步服务器例子
        static void AsyncServer()
        {
            Socket listen = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            IPAddress ipAdd =  IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEnd = new IPEndPoint(ipAdd,9999);
            listen.Bind(ipEnd);
            listen.Listen(0);

            listen.BeginAccept(AcceptCallBack, listen);
            Console.WriteLine("[Server] Begin Accept");
            Console.ReadLine();
        }

        static void AcceptCallBack(IAsyncResult ar)
        {
            try
            {
                Socket acceptSocket = (Socket)ar.AsyncState;
                Socket clientSocket = acceptSocket.EndAccept(ar);

                ClientState clientState = new ClientState();
                clientState.socket = clientSocket;
                ClientStates.Add(clientSocket, clientState);
                clientSocket.BeginReceive(clientState.readBuff, 0, 1024, 0, ReceiveCallBack, clientState);
                Console.WriteLine("[Server] Accept Success");
                acceptSocket.BeginAccept(AcceptCallBack, acceptSocket);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Accept Fail" + e);
                throw;
            }
        }

        static void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                ClientState clientState = (ClientState)ar.AsyncState;
                Socket clientSocket = clientState.socket;
                int count = clientSocket.EndReceive(ar);
                if (count ==0)
                {
                    clientSocket.Close();
                    ClientStates.Remove(clientSocket);
                    return;
                }

                string ipStr = clientState.socket.RemoteEndPoint.ToString() + ":";
                string receiveStr = System.Text.Encoding.Default.GetString(clientState.readBuff, 0, count);
                receiveStr = ipStr + receiveStr;
                byte[] sendBytes = System.Text.Encoding.Default.GetBytes(receiveStr);
                foreach (var item in ClientStates)
                {
                    item.Key.Send(sendBytes);
                }
                //clientSocket.Send(sendBytes);
                clientSocket.BeginReceive(clientState.readBuff, 0, 1024, 0, ReceiveCallBack, clientState);
            }
            catch (SocketException e)
            {
                Console.WriteLine("[Server] Receive Fail");
                throw;
            }
        }



        //同步服务器例子
        static void SyncServer()
        {
            Console.WriteLine("Hello World!");

            Socket listen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress ipAdr = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEp = new IPEndPoint(ipAdr, 9999);

            listen.Bind(ipEp);

            listen.Listen(0);
            Console.WriteLine("[Server] is Start");
            while (true)
            {
                Socket connfd = listen.Accept();
                Console.WriteLine("[Server] is Accept");
                byte[] readBuff = new byte[1024];
                int count = connfd.Receive(readBuff);
                string timeContent = System.DateTime.Now.ToString();
                string readStr = System.Text.Encoding.Default.GetString(readBuff, 0, count);
                Console.WriteLine("[Server] Receive " + readStr);
                byte[] sendBytes = System.Text.Encoding.Default.GetBytes(readStr + timeContent);
                connfd.Send(sendBytes);
            }
        }
    }


    
}
