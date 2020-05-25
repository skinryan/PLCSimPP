﻿using System;
using System.Collections.Generic;
using System.Text;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Events;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Interfaces.Services;
using BCI.PLCSimPP.Communication;
using BCI.PLCSimPP.Communication.EventArguments;
using BCI.PLCSimPP.Communication.Models;
using BCI.PLCSimPP.Communication.Support;
using Prism.Events;

namespace BCI.PLCSimPP.Service.Services
{
    public class PortConnection
    {
        private TcpIpServerConnection mTcpReceiver;
        private TcpIpServerConnection mTcpSender;
        private ILogService mlogger;
        private IEventAggregator mEventAggr;

        public delegate void TransportLayerDataReceivedEventHandler(object sender, TransportLayerDataReceivedEventArgs e);

        public event TransportLayerDataReceivedEventHandler OnMsgReceived;

        public int SernderPort { get; private set; }

        public int ReceiverPort { get; private set; }

        public string IpAddress { get; private set; }

        public int MasterPortNumber { get; private set; }

        public PortConnection(int portNumber, ILogService logger, IEventAggregator eventAggr)
        {
            mlogger = logger;
            mEventAggr = eventAggr;

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
            //mTcpSender.TransportLayerDataReceivedEvent += TcpSender_OnDataReceivedEvent;
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
            //mlogger.LogSys(string.Format("{0}:{1} {2}", IpAddress, SernderPort, e.State));

        }

        private void TcpReceiver_OnStateChangedEvent(object sender, TransportLayerStateChangedEventArgs e)
        {
            //mlogger.LogSys(string.Format("{0}:{1} {2}", IpAddress, ReceiverPort, e.State));
            ConnInfo cs = new ConnInfo() { IsConnected = e.State == ConnectionState.Open, Port = MasterPortNumber };
            mEventAggr.GetEvent<ConnectionStatusEvent>().Publish(cs);
        }

        private void TcpReceiver_OnDataReceivedEvent(object sender, TransportLayerDataReceivedEventArgs e)
        {
            //mlogger.LogSys($"Port {e.ReceivedMsg.Port} received msg {e.ReceivedMsg}");
            OnMsgReceived(sender, e);
        }

        public void SendMsg(IMessage msg)
        {
            mTcpSender.Write(msg);
        }
    }
}
