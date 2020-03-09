using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using PLCSimPP.Communication;
using PLCSimPP.Communication.EventArguments;
using PLCSimPP.Communication.Models;
using PLCSimPP.Communication.Support;

namespace TestTcp
{
    class Program
    {
        static TcpIpServerConnection tcpReceiver;
        static TcpIpServerConnection tcpSender;
        static void Main(string[] args)
        {
            //SmartConnection sc = new SmartConnection();

            //CmdMsg msg = new CmdMsg();
            //msg.Command = "1021";
            //msg.Unit = "0000000001";
            //msg.Param = "B";

            //sc.Send(msg);

            //receiver port 1281
            tcpReceiver = new TcpIpServerConnection();
            tcpReceiver.TransportLayerDataReceivedEvent += TcpReceiver_TransportLayerDataReceivedEvent;
            //tcpReceiver.TransportLayerStateChangedEvent += TcpReceiver_TransportLayerStateChangedEvent;

            //sender port 1280 
            tcpSender = new TcpIpServerConnection();
            tcpSender.TransportLayerDataReceivedEvent += TcpReceiver_TransportLayerDataReceivedEvent;
            tcpSender.TransportLayerStateChangedEvent += TcpSender_TransportLayerStateChangedEvent;

            tcpSender.Open("Unit1 Sender", "128.0.15.2", 1280);
            tcpReceiver.Open("Unit1 Receiver", "128.0.15.2", 1281);

            //CmdMsg msg = new CmdMsg();

            //msg.Command = "1021";
            //msg.Unit = "0000000001";
            //msg.Param = "09119282";

            //tcpSender.Write(msg);
            //Thread t0 = new Thread(delegate ()
            //{

            //    TcpServer myserver = new TcpServer("128.0.15.2", 1280);
            //});
            //t0.Start();

            //Thread t1 = new Thread(delegate ()
            //{

            //    TcpServer myserver = new TcpServer("128.0.15.2", 1281);
            //});
            //t1.Start();


            Console.WriteLine("Server Started...!");


            while (true)
            {
                //Thread.Sleep(1000);
                //if (tcpSender.State == ConnectionState.Open)
                //{
                //    Unit1_1001Rep();
                //}

                Console.ReadLine();
            }
        }

        private static void TcpSender_TransportLayerStateChangedEvent(object sender, TransportLayerStateChangedEventArgs e)
        {

            Console.WriteLine(e.State);
        }

        //
        private static void TcpReceiver_TransportLayerStateChangedEvent(object sender, TransportLayerStateChangedEventArgs e)
        {
            Console.WriteLine(e.State);
        }

        private static void TcpReceiver_TransportLayerDataReceivedEvent(object sender, TransportLayerDataReceivedEventArgs e)
        {
            var ss = e.ReceivedString;

            //string hex = BitConverter.ToString(ss.RawData);

            //Console.WriteLine(ss.Unit + "|" + ss.Command + "|" + ss.Param);
            if (ss.Command == "0004")
            {
                Unit1_1001Rep();
            }
            //throw new NotImplementedException();
        }

        private static void Unit1_1001Rep()
        {
            if (tcpSender.State == ConnectionState.Open)
            {
                string[] addrArray = { "0000000001",
                                       "0000000002",
                                       "0000000004",
                                       "0000000008",
                                       "0000000010",
                                       "0000000020",
                                       "0000000040"};

                foreach (var item in addrArray)
                {
                    CmdMsg cmd = new CmdMsg()
                    {
                        Command = "1001",
                        Param = "A",
                        Unit = item
                    };
                    
                    tcpSender.Write(cmd);
                }

            }

        }
    }
}
