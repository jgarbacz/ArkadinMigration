using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.IO;

namespace TcpTest
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("TcpTest.exe <t#> s <port> <client_size> <server_size>");
                Console.WriteLine("TcpTest.exe <t#> c <port> <client_size> <server_size> <message_count> <server_ip>");
                return;
            }

            if (args[0].Equals("t1"))
            {
                if (args[1].Equals("s"))
                {
                    int port = Int32.Parse(args[2]);
                    int clientSize = Int32.Parse(args[3]);
                    int serverSize = Int32.Parse(args[4]);
                    Server(port, clientSize, serverSize);
                }
                else if (args[1].Equals("c"))
                {
                    int port = Int32.Parse(args[2]);
                    int clientSize = Int32.Parse(args[3]);
                    int serverSize = Int32.Parse(args[4]);
                    int messageCount = Int32.Parse(args[5]);
                    string server = args[6];
                    Client(server, port, clientSize, serverSize, messageCount);
                }
                else Console.WriteLine("bad c or s");
            }
            else if (args[0].Equals("t2"))
            {
                if (args[1].Equals("s"))
                {
                    int port = Int32.Parse(args[2]);
                    int clientSize = Int32.Parse(args[3]);
                    int serverSize = Int32.Parse(args[4]);
                    Server2(port, clientSize, serverSize);
                }
                else if (args[1].Equals("c"))
                {
                    int port = Int32.Parse(args[2]);
                    int clientSize = Int32.Parse(args[3]);
                    int serverSize = Int32.Parse(args[4]);
                    int messageCount = Int32.Parse(args[5]);
                    string server = args[6];
                    Client(server, port, clientSize, serverSize, messageCount);
                }
                else Console.WriteLine("bad c or s");
            }
            else if (args[0].Equals("t3"))
            {
                if (args[1].Equals("s"))
                {
                    int port = Int32.Parse(args[2]);
                    int clientSize = Int32.Parse(args[3]);
                    int serverSize = Int32.Parse(args[4]);
                    Server3(port, clientSize, serverSize);
                }
                else if (args[1].Equals("c"))
                {
                    int port = Int32.Parse(args[2]);
                    int clientSize = Int32.Parse(args[3]);
                    int serverSize = Int32.Parse(args[4]);
                    int messageCount = Int32.Parse(args[5]);
                    string server = args[6];
                    Client3(server, port, clientSize, serverSize, messageCount);
                    Client3(server, port, clientSize, serverSize, messageCount);
                    Client3(server, port, clientSize, serverSize, messageCount);
                    Client3(server, port, clientSize, serverSize, messageCount);
                    Client3(server, port, clientSize, serverSize, messageCount);
                    Client3(server, port, clientSize, serverSize, messageCount);
                    Client3(server, port, clientSize, serverSize, messageCount);
                    Client3(server, port, clientSize, serverSize, messageCount);
                    Client3(server, port, clientSize, serverSize, messageCount);
                    Client3(server, port, clientSize, serverSize, messageCount);
                }
                else Console.WriteLine("bad c or s");
            }
            else
            {
                Console.WriteLine("unknown test");
            }
        }

        public static void Server(int serverPort, int clientSize, int serverSize)
        {
            for (; ; )
            {
                byte[] receiveData = new byte[clientSize];
                byte[] sendData = new byte[serverSize];
                IPEndPoint ipep = new IPEndPoint(IPAddress.Any, serverPort);
                Socket newsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                newsock.Bind(ipep);
                newsock.Listen(10);
                Console.WriteLine("Waiting for a client on port:" + serverPort);
                Socket client = newsock.Accept();
                IPEndPoint clientep = (IPEndPoint)client.RemoteEndPoint;
                Console.WriteLine("Connected with {0} at port {1}", clientep.Address, clientep.Port);
                long totalRecieved = 0;
                long totalSent = 0;
                long totalRt = 0;
                while (true)
                {
                    int recv = client.Receive(receiveData);
                    totalRecieved += recv;
                    if (recv == 0) break;
                    //Console.WriteLine("Recieved {0} bytes",recv);
                    client.Send(sendData, sendData.Length, SocketFlags.None);
                    totalSent += sendData.Length;
                    totalRt += 1;
                }
                Console.WriteLine("Disconnected from {0}, sent bytes={1},recv bytes={2}, roundtrips={3}", clientep.Address, totalSent, totalRecieved, totalRt);
                client.Close();
                newsock.Close();
            }
        }



        static void Client(String server, int port, int clientSize, int serverSize, int messageCount)
        {
            byte[] receiveData = new byte[serverSize];
            byte[] sendData = new byte[clientSize];

            try
            {
                TcpClient client = new TcpClient(server, port);
                NetworkStream stream = client.GetStream();
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                for (int i = 0; i < messageCount; i++)
                {
                    stream.Write(sendData, 0, sendData.Length);
                    //Console.WriteLine("#{1} Sent: {0} bytes", sendData.Length,i);
                    int bytes = stream.Read(receiveData, 0, receiveData.Length);
                    //Console.WriteLine("#{1} Received: {0} bytes", bytes,i);
                }

                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;

                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                Console.WriteLine("RunTime " + elapsedTime);

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }

        public static void Server2(int serverPort, int clientSize, int serverSize)
        {
            for (; ; )
            {
                byte[] receiveData = new byte[clientSize];
                byte[] sendData = new byte[serverSize];

                var listener = new TcpListener(serverPort);
                listener.Start();
                Console.WriteLine("Waiting for a client on port:" + serverPort);
                TcpClient client = listener.AcceptTcpClient();
                NetworkStream stream = client.GetStream();

                Console.WriteLine("Connected with client at port {0}", serverPort);
                long totalRecieved = 0;
                long totalSent = 0;
                long totalRt = 0;
                while (true)
                {
                    int recv = stream.Read(receiveData, 0, receiveData.Length);
                    totalRecieved += recv;
                    if (recv == 0) break;
                    //Console.WriteLine("Recieved {0} bytes",recv);
                    stream.Write(sendData, 0, sendData.Length);
                    totalSent += sendData.Length;
                    totalRt += 1;
                }
                Console.WriteLine("Disconnected from client, sent bytes={0},recv bytes={1}, roundtrips={2}", totalSent, totalRecieved, totalRt);
                client.Close();
                listener.Stop();
            }
        }


        //----------------------- t3

        static void Client3(String server, int port, int clientSize, int serverSize, int messageCount)
        {
            byte[] receiveData = new byte[serverSize];
            byte[] sendData = new byte[clientSize];
            try
            {
                TcpClient client = new TcpClient(server, port);
                client.NoDelay = true;
                client.SendBufferSize = 10 * 1000 * 1024;
                client.ReceiveBufferSize = 10 * 1000 * 1024;
                NetworkStream stream = client.GetStream();
                BinaryReader binaryReader = new BinaryReader(stream);
                BinaryWriter binaryWriter = new BinaryWriter(stream);
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                for (int i = 0; i < messageCount; i++)
                {
                    binaryWriter.Write(sendData);
                    //Console.WriteLine("#{1} Sent: {0} bytes", sendData.Length,i);
                    int bytes = binaryReader.Read(receiveData, 0, receiveData.Length);
                    //Console.WriteLine("#{1} Received: {0} bytes", bytes,i);
                }

                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;

                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                Console.WriteLine("RunTime " + elapsedTime);

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }

        public static void Server3(int serverPort, int clientSize, int serverSize)
        {
            TcpListener listener = new TcpListener(serverPort);
            listener.Start();
            for (; ; )
            {
                byte[] receiveData = new byte[clientSize];
                byte[] sendData = new byte[serverSize];
                Console.WriteLine("Waiting for a client on port:" + serverPort);
                TcpClient client = listener.AcceptTcpClient();
                client.NoDelay = true;
                client.SendBufferSize = 10 * 1000 * 1024;
                client.ReceiveBufferSize = 10 * 1000 * 1024;

                NetworkStream stream = client.GetStream();
                BinaryReader binaryReader = new BinaryReader(stream);
                BinaryWriter binaryWriter = new BinaryWriter(stream);
                Console.WriteLine("Connected with client at port {0}", serverPort);
                long totalRecieved = 0;
                long totalSent = 0;
                long totalRt = 0;
                try
                {
                    while (true)
                    {
                        int recv = binaryReader.Read(receiveData, 0, receiveData.Length);
                        totalRecieved += recv;
                        //Console.WriteLine("Recieved {0} bytes",recv);

                        // bulk: .36, 1.29...
                        binaryWriter.Write(sendData, 0, sendData.Length); 

                        //byte by byte: 2.25, 3.35...
                        //for (int i = 0; i < sendData.Length; i++) binaryWriter.Write(sendData, i, 1); 

                        //byte by byte copy+bulk: .41,1.29...
                        //MemoryStream tmpMemoryStream = new MemoryStream();
                        //BinaryWriter tmpBinaryWriter = new BinaryWriter(tmpMemoryStream);
                        //for (int i = 0; i < sendData.Length; i++)
                        //{
                        //    tmpBinaryWriter.Write(sendData, i, 1); 
                        //}
                        //binaryWriter.Write(tmpMemoryStream.GetBuffer(), 0, (int)tmpMemoryStream.Length); 

                        totalSent += sendData.Length;
                        totalRt += 1;
                    }
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine("Disconnected from client, sent bytes={0},recv bytes={1}, roundtrips={2}", totalSent, totalRecieved, totalRt);
                    client.Close();
                }
            }
            listener.Stop();
        }
    }
}
