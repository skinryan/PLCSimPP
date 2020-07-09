using System;
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

        /// <summary>
        /// Sender Port number
        /// </summary>
        public int SenderPort { get; private set; }

        /// <summary>
        /// Receiver Port nubmer
        /// </summary>
        public int ReceiverPort { get; private set; }

        /// <summary>
        /// IP Address
        /// </summary>
        public string IpAddress { get; private set; }

        /// <summary>
        /// Master Port Number
        /// </summary>
        public int MasterPortNumber { get; private set; }


        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="portNumber"></param>
        /// <param name="logger"></param>
        /// <param name="eventAggr"></param>
        public PortConnection(int portNumber, ILogService logger, IEventAggregator eventAggr)
        {
            mlogger = logger;
            mEventAggr = eventAggr;

            MasterPortNumber = portNumber;
            if (portNumber == 1)
            {
                IpAddress = CommConst.PORT1_IP;
                SenderPort = CommConst.PORT1_SEND;
                ReceiverPort = CommConst.PORT1_RECEIVE;
            }

            if (portNumber == 2)
            {
                IpAddress = CommConst.PORT2_IP;
                SenderPort = CommConst.PORT2_SEND;
                ReceiverPort = CommConst.PORT2_RECEIVE;
            }

            if (portNumber == 3)
            {
                IpAddress = CommConst.PORT3_IP;
                SenderPort = CommConst.PORT3_SEND;
                ReceiverPort = CommConst.PORT3_RECEIVE;
            }

            mTcpSender = new TcpIpServerConnection();
            mTcpSender.TransportLayerStateChangedEvent += TcpSender_OnStateChangedEvent;

            mTcpReceiver = new TcpIpServerConnection();
            mTcpReceiver.TransportLayerDataReceivedEvent += TcpReceiver_OnDataReceivedEvent;
            mTcpReceiver.TransportLayerStateChangedEvent += TcpReceiver_OnStateChangedEvent;
        }

        /// <summary>
        /// Open Connect
        /// </summary>
        public void OpenConnect()
        {
            mTcpSender.Open($"Master Port {MasterPortNumber} Sender", IpAddress, SenderPort);
            mTcpReceiver.Open($"Master Port {MasterPortNumber} Receiver", IpAddress, ReceiverPort);
        }

        /// <summary>
        /// Close Connect
        /// </summary>
        public void CloseConnect()
        {
            mTcpSender.Close();
            mTcpReceiver.Close();
        }

        /// <summary>
        /// TCP sender State Changed Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TcpSender_OnStateChangedEvent(object sender, TransportLayerStateChangedEventArgs e)
        {
            mlogger.LogSys($"{IpAddress}:{SenderPort} {e.State}");
        }

        /// <summary>
        /// TCP receiver State Changed Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TcpReceiver_OnStateChangedEvent(object sender, TransportLayerStateChangedEventArgs e)
        {
            mlogger.LogSys($"{IpAddress}:{ReceiverPort} {e.State}");
            ConnInfo cs = new ConnInfo() { IsConnected = e.State == ConnectionState.Open, Port = MasterPortNumber };
            mEventAggr.GetEvent<ConnectionStatusEvent>().Publish(cs);
        }

        /// <summary>
        /// Tcp Receiver Data Received Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TcpReceiver_OnDataReceivedEvent(object sender, TransportLayerDataReceivedEventArgs e)
        {
            OnMsgReceived(sender, e);
        }

        /// <summary>
        /// send message
        /// </summary>
        /// <param name="msg"></param>
        public void SendMsg(IMessage msg)
        {
            mTcpSender.Write(msg);
        }
    }
}
