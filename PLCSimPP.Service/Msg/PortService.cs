using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using CommonServiceLocator;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Interfaces.Services;
using BCI.PLCSimPP.Comm.Models;
using BCI.PLCSimPP.Communication.Models;
using Prism.Events;

namespace BCI.PLCSimPP.Service.Services
{
    public class PortService : IPortService
    {
        private readonly ILogService mLogger;
        private readonly IEventAggregator mEventAggr;
        private ConcurrentQueue<IMessage> mReceivedQueue;

        public PortConnection MasterPort_1 { get; set; }
        public PortConnection MasterPort_2 { get; set; }
        public PortConnection MasterPort_3 { get; set; }

        public PortService()
        {
            mReceivedQueue = new ConcurrentQueue<IMessage>();
        }

        public void Connect()
        {
            MasterPort_1.OpenConnect();
            MasterPort_2.OpenConnect();
            MasterPort_3.OpenConnect();
        }

        public void Disconnect()
        {
            MasterPort_1.CloseConnect();
            MasterPort_2.CloseConnect();
            MasterPort_3.CloseConnect();
            mReceivedQueue = new ConcurrentQueue<IMessage>();
        }

        public PortService(ILogService logger, IEventAggregator aggregator)
        {
            mLogger = logger;
            mEventAggr = aggregator;

            MasterPort_1 = new PortConnection(1, mLogger, mEventAggr);
            MasterPort_2 = new PortConnection(2, mLogger, mEventAggr);
            MasterPort_3 = new PortConnection(3, mLogger, mEventAggr);

            MasterPort_1.OnMsgReceived += MasterPort_OnMsgReceived;
            MasterPort_2.OnMsgReceived += MasterPort_OnMsgReceived;
            MasterPort_3.OnMsgReceived += MasterPort_OnMsgReceived;
        }

        private void MasterPort_OnMsgReceived(object sender, Communication.EventArguments.TransportLayerDataReceivedEventArgs e)
        {
            mReceivedQueue.Enqueue(e.ReceivedMsg);
        }


        public void SendMsg(IMessage msg)
        {
            if (msg.Port == 1)
            {
                MasterPort_1.SendMsg(msg);
            }
            else if (msg.Port == 2)
            {
                MasterPort_2.SendMsg(msg);
            }
            else if (msg.Port == 3)
            {
                MasterPort_3.SendMsg(msg);
            }
        }

        public bool TryDequeue(out IMessage msg)
        {
            return mReceivedQueue.TryDequeue(out msg);
        }
    }
}

