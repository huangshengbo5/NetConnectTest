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
        public  byte [] readBuff = new byte[1024];
    }

    class  SelectNet
    {
        private static Socket listenFd;
        static Dictionary<Socket,ClientState> clients = new Dictionary<Socket, ClientState>();

        public static void MainAccess()
        {
            listenFd = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            IPAddress ipAdr = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEnd = new IPEndPoint(ipAdr,9999);
            listenFd.Bind(ipEnd);
            listenFd.Listen(0);
            Console.WriteLine("[Server] is Start");

            List<Socket> checkRead = new List<Socket>();

            while (true)
            {
                checkRead.Clear();
                checkRead.Add(listenFd);
                foreach (var VARIABLE in clients.Values)
                {
                    checkRead.Add(VARIABLE.socket);
                }

                Socket.Select(checkRead, null, null, 1000);
                foreach (var VARIABLE in checkRead)
                {
                    if (VARIABLE == listenFd)
                    {
                        ReadListenfd(VARIABLE);
                    }
                    else
                    {
                        ReadClientfd(VARIABLE);
                    }
                }
            }
        }

        public static void ReadListenfd(Socket s)
        {
            Console.WriteLine("Accept");
            Socket clientFd = s.Accept();
            ClientState state = new ClientState();
            state.socket = s;
            clients.Add(clientFd,state);
        }

        public static void ReadClientfd(Socket s)
        {
            ClientState state = clients[s];
            int count = 0;
            try
            {
                count = s.Receive(state.readBuff);
            }
            catch (SocketException e)
            {
                s.Close();
                clients.Remove(s);
                Console.WriteLine(e);
                throw;
            }

            if (count == 0)
            {
                s.Close();
                clients.Remove(s);
            }

            string receStr = System.Text.Encoding.Default.GetString(state.readBuff, 0, count);
            byte[] sendBytes = System.Text.Encoding.Default.GetBytes(receStr);
            foreach (var VARIABLE in clients.Values)
            {
                VARIABLE.socket.Send(sendBytes);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            
        }


    }
}
