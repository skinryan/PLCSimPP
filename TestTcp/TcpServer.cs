using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TestTcp
{
    public class TcpServer
    {
        TcpListener server = null;
        public TcpServer(string ip, int port)
        {
            IPAddress localAddr = IPAddress.Parse(ip);
            server = new TcpListener(localAddr, port);
            server.Start();
            StartListener();
        }

        public void StartListener()
        {
            try
            {
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Client :{0} Connected to Server :{1}", client.Client.RemoteEndPoint.ToString(), client.Client.LocalEndPoint.ToString());

                    Thread t = new Thread(new ParameterizedThreadStart(HandleDevice));
                    t.Start(client);
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                server.Stop();
            }
        }

        public static byte[] GetSubBytes(byte[] ba, int offset, int length)
        {
            byte[] sba = new byte[length];
            Array.Copy(ba, offset, sba, 0, length);
            return sba;
        }

        private void HandleDevice(object obj)
        {
            TcpClient client = (TcpClient)obj;
            var stream = client.GetStream();

            Console.WriteLine("Client :{0} received msg.", client.Client.RemoteEndPoint.ToString());


            byte[] bytes = new byte[256];
            try
            {
                var len = stream.Read(bytes);

                Console.WriteLine("Received {0} bytes", len);

                var realbytes = GetSubBytes(bytes, 0, len);

                string t = "";
                foreach (var item in realbytes)
                {
                    t += item.ToString("X2");
                }

                Console.WriteLine(t);

                //while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                //{
                //    string hex = BitConverter.ToString(bytes);

                //    data = Encoding.ASCII.GetString(bytes, 0, i);

                //    Console.WriteLine("{1}: Received: {0}", data, Thread.CurrentThread.ManagedThreadId);
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.ToString());
                client.Close();
            }

        }
    }
}
