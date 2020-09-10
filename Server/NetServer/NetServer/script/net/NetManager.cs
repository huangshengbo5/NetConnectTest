using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Reflection;


    class NetManager
    {
        //监听socket
        public static Socket listenfd;
        //客户端Socket及状态信息
        public  static  Dictionary<Socket,ClientState> clients = new Dictionary<Socket, ClientState>();

        //Select的检查列表
        static List<Socket>checkRead = new List<Socket>();

        //ping间隔
        public static long pingInterval = 30;


        public static void StartLoop(int listenPort)
        {
            listenfd = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);

            IPAddress ipAdr = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEp = new IPEndPoint(ipAdr,listenPort);

            listenfd.Bind(ipEp);

            //listen
            listenfd.Listen(0);
            Console.WriteLine("[服务器]启动成功！");

            while (true)
            {
                ResetCheckRead();
                Socket.Select(checkRead,null,null,1000);

                for (int i = checkRead.Count-1; i >=0; i--)
                {
                    Socket s = checkRead[i];
                    if (s == listenfd)
                    {
                        ReadListenfd(s);
                    }
                    else
                    {
                        ReadClientfd(s);
                    }
                }

                Timer();
            }
        }

        public static void ResetCheckRead()
        {
            checkRead.Clear();
            checkRead.Add(listenfd);
            foreach (ClientState state in clients.Values)
            {
                checkRead.Add(state.socket);
            }
        }

        public static void ReadListenfd(Socket s)
        {
            try
            {
                Socket clientfd = s.Accept();
                Console.WriteLine("Accept "+ clientfd.RemoteEndPoint.ToString());
                ClientState state = new ClientState();
                state.socket = clientfd;
                state.lastPingTime = GetTimeStamp();
                clients.Add(clientfd, state);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Accept fail "+e);
                throw;
            }
        }

        public static void ReadClientfd(Socket s)
        {
            ClientState state = clients[s];
            ByteArray readbuff = state.readBuff;
            //接收

            int count = 0;

            if (readbuff.remain>=0)
            {
                OnReceiveData(state);
                readbuff.MoveBytes();
            }

            if (readbuff.remain <=0)
            {
                Console.WriteLine("Receive fail , maybe msg length > buff capacity");
                Close(state);
                return;
            }

            try
            {
                count = s.Receive(readbuff.bytes, readbuff.writeIndex, readbuff.remain, 0);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Receive SocketException "+e);
                Close(state);
                throw;
            }
            //客户端关闭
            if (count <=0)
            {
                Console.WriteLine("Socket close"+s.RemoteEndPoint.ToString());
                Close(state);
                return;
            }

            //消息处理
            readbuff.writeIndex += count;
            //处理二进制消息
            OnReceiveData(state);
            //移动缓冲区
            readbuff.CheckAndMoveBytes();
        }


        public static void Close(ClientState state)
        {
            MethodInfo met = typeof(EventHandler).GetMethod("OnDisconnect");
            object[] obj = {state};
            met.Invoke(null, obj);
            state.socket.Close();
            clients.Remove(state.socket);
        }
        public static void OnReceiveData(ClientState state)
        {
            ByteArray readBuff = state.readBuff;
            byte[] bytes = readBuff.bytes;
            //消息长度
            if (readBuff.length <2 )
            {
                return;
            }

            Int16 bodyLength = (Int16) ((bytes[readBuff.readIndex + 1 ]<<8) | bytes[readBuff.readIndex]);

            //消息体
            if (readBuff.length<bodyLength)
            {
                return;
            }

            readBuff.readIndex += 2;
            //解析协议名
            int nameCount = 0;
            string protoName = MsgBase.DecodeName(readBuff.bytes, readBuff.readIndex, out nameCount);

            if (protoName == "")
            {
                Console.WriteLine("OnReceiveData MsgBase.DecodeName Fail");
                Close(state);
            }

            readBuff.readIndex += nameCount;
            //解析协议体
            int bodyCount = bodyLength - nameCount;
            MsgBase msgBase = MsgBase.Decode(protoName,readBuff.bytes,readBuff.readIndex,bodyCount);

            readBuff.readIndex += bodyCount;
            readBuff.CheckAndMoveBytes();

            //分发消息
            MethodInfo mi = typeof(SysMsgHandler).GetMethod(protoName);
            object[] ob = { state, msgBase };
            Console.WriteLine("Receive" + protoName);
            if (mi != null)
            {
                mi.Invoke(null, ob);
            }
            else
            {
                Console.WriteLine("OnReceive Invoke Fail " + protoName);
            }

            //继续读取消息
            if (readBuff.length >2)
            {
                OnReceiveData(state);
            }
        }

        public static void Timer()
        {
            //消息分发
            MethodInfo mi = typeof(EventHandler).GetMethod("OnTimer");
            object[] ob = { };
            mi.Invoke(null, ob);
        }


        public static void Send(ClientState cs, MsgBase msg)
        {
            if (cs == null)
            {
                return;
            }

            if (!cs.socket.Connected)
            {
                return;
            }

            //数据编码
            byte[] nameBytes = MsgBase.EncodeName(msg);
            byte[] bodyBytes = MsgBase.Encode(msg);
            int len = nameBytes.Length + bodyBytes.Length;
            byte[] sendBytes = new byte[2 + len];

            //组装长度
            sendBytes[0] = (byte) (len % 256);
            sendBytes[1] = (byte) (len / 256);

            //组装名字
            Array.Copy(nameBytes,0, sendBytes,2,nameBytes.Length);
            //组装消息体
            Array.Copy(bodyBytes,0,sendBytes,2+nameBytes.Length,bodyBytes.Length);
            //暂时不设置回调
            try
            {
                cs.socket.BeginSend(sendBytes, 0, sendBytes.Length, 0, null, null);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Socket Close On BeginSend"+e.ToString());
                throw;
            }
        }

        //获取时间戳
        public static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }
    }
