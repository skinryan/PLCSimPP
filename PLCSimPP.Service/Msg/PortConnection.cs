using System;
using System.Collections.Generic;
using System.Text;
using PLCSimPP.Comm.Constants;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Comm.Interfaces.Services;
using PLCSimPP.Communication;
using PLCSimPP.Communication.EventArguments;
using PLCSimPP.Communication.Models;

namespace PLCSimPP.Service.Services
{
    public class PortConnection
    {
        private TcpIpServerConnection mTcpReceiver;
        private TcpIpServerConnection mTcpSender;
        private ILogService mlogger;

        public delegate void TransportLayerDataReceivedEventHandler(object sender, TransportLayerDataReceivedEventArgs e);

        public event TransportLayerDataReceivedEventHandler OnMsgReceived;

        public int SernderPort { get; private set; }

        public int ReceiverPort { get; private set; }

        public string IpAddress { get; private set; }

        public int MasterPortNumber { get; private set; }

        public PortConnection(int portNumber, ILogService logger)
        {
            mlogger = logger;

            MasterPortNumber = portNumber;
            if (portNumber == 1)
            {
                IpAddress = CommConst.PORT1_IP;
                SernderPort = CommConst.PORT1_SEND;
                ReceiverPort = CommConst.PORT1_RECEIVE;
            }

            if (portNumber == 2)
            {
                IpAddress = CommConst.PORT2_IP;
                SernderPort = CommConst.PORT2_SEND;
                ReceiverPort = CommConst.PORT2_RECEIVE;
            }

            if (portNumber == 3)
            {
                IpAddress = CommConst.PORT3_IP;
                SernderPort = CommConst.PORT3_SEND;
                ReceiverPort = CommConst.PORT3_RECEIVE;
            }

            mTcpSender = new TcpIpServerConnection();
            mTcpSender.TransportLayerStateChangedEvent += TcpSender_OnStateChangedEvent;

            mTcpReceiver = new TcpIpServerConnection();
            mTcpReceiver.TransportLayerDataReceivedEvent += TcpReceiver_OnDataReceivedEvent;
            mTcpReceiver.TransportLayerStateChangedEvent += TcpReceiver_OnStateChangedEvent;

            // tcp/ip servier started
        }

        public void OpenConnect()
        {
            mTcpSender.Open(string.Format("Master Port {0} Sender", MasterPortNumber), IpAddress, SernderPort);
            mTcpReceiver.Open(string.Format("Master Port {0} Receiver", MasterPortNumber), IpAddress, ReceiverPort);


        }

        public void CloseConnect()
        {
            mTcpSender.Close();
            mTcpReceiver.Close();
        }

        private void TcpSender_OnStateChangedEvent(object sender, TransportLayerStateChangedEventArgs e)
        {
            mlogger.LogSys(string.Format("{0}:{1} {2}", IpAddress, SernderPort, e.State));
        }

        private void TcpReceiver_OnStateChangedEvent(object sender, TransportLayerStateChangedEventArgs e)
        {
             mlogger.LogSys(string.Format("{0}:{1} {2}", IpAddress, ReceiverPort, e.State));
        }

        private void TcpReceiver_OnDataReceivedEvent(object sender, TransportLayerDataReceivedEventArgs e)
        {
            OnMsgReceived(sender, e);
        }

        public void SendMsg(IMessage msg)
        {
            mTcpSender.Write(msg);
        }
    }
}
