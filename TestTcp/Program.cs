using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using PLCSimPP.Comm.Models;
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

            //int _1 = 0x0000000001;
            //int _2 = 0x0000000002;
            //int _3 = 0x0000000004;
            //int _4 = 0x0000000008;
            //int _5 = 0x0000000010;
            //int _6 = 0x0000000020;
            //int _7 = 0x0000000040;


            //int _10 = 0x0000000080;


            //Console.WriteLine(_1|_2|_3|_4|_5|_6|_7);
            //Console.WriteLine(_7 | 127);



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

            var tcp1282 = new TcpIpServerConnection();
            tcp1282.TransportLayerDataReceivedEvent += TcpReceiver_TransportLayerDataReceivedEvent;
            //tcpReceiver.TransportLayerStateChangedEvent += TcpReceiver_TransportLayerStateChangedEvent;

            //sender port 1280 
            var tcp1283 = new TcpIpServerConnection();
            tcp1283.TransportLayerDataReceivedEvent += TcpReceiver_TransportLayerDataReceivedEvent;
            tcp1283.TransportLayerStateChangedEvent += TcpSender_TransportLayerStateChangedEvent;

            tcp1282.Open("Unit1 Sender", "128.0.15.3", 1282);
            tcp1283.Open("Unit1 Receiver", "128.0.15.3", 1283);

            var tcp1284 = new TcpIpServerConnection();
            tcp1284.TransportLayerDataReceivedEvent += TcpReceiver_TransportLayerDataReceivedEvent;
            //tcpReceiver.TransportLayerStateChangedEvent += TcpReceiver_TransportLayerStateChangedEvent;

            //sender port 1280 
            var tcp1285 = new TcpIpServerConnection();
            tcp1285.TransportLayerDataReceivedEvent += TcpReceiver_TransportLayerDataReceivedEvent;
            tcp1285.TransportLayerStateChangedEvent += TcpSender_TransportLayerStateChangedEvent;

            tcp1282.Open("Unit1 Sender", "128.0.15.4", 1284);
            tcp1283.Open("Unit1 Receiver", "128.0.15.4", 1285);

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
            var ss = e.ReceivedMsg;

            //string hex = BitConverter.ToString(ss.RawData);

            Console.WriteLine(ss.UnitAddr + "|" + ss.Command + "|" + ss.Param);
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
                    MsgCmd cmd = new MsgCmd()
                    {
                        Command = "1001",
                        Param = "A",
                        UnitAddr = item
                    };

                    tcpSender.Write(cmd);
                }

            }

        }
    }
}
