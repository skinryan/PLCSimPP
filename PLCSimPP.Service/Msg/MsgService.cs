using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using CommonServiceLocator;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Comm.Interfaces.Services;
using PLCSimPP.Comm.Models;
using PLCSimPP.Communication.Models;

namespace PLCSimPP.Service.Services
{
    public class MsgService : IMsgService
    {
        private readonly ILogService mLogger;
        private ConcurrentQueue<IMessage> mReceivedQueue;

        public PortConnection MasterPort_1 { get; set; }
        public PortConnection MasterPort_2 { get; set; }
        public PortConnection MasterPort_3 { get; set; }

        public MsgService()
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

        public MsgService(ILogService logger)
        {
            mLogger = logger;

            MasterPort_1 = new PortConnection(1, mLogger);
            MasterPort_2 = new PortConnection(2, mLogger);
            MasterPort_3 = new PortConnection(3, mLogger);

            MasterPort_1.OnMsgReceived += MasterPort_OnMsgReceived;
            MasterPort_2.OnMsgReceived += MasterPort_OnMsgReceived;
            MasterPort_3.OnMsgReceived += MasterPort_OnMsgReceived;
        }

        private void MasterPort_OnMsgReceived(object sender, Communication.EventArguments.TransportLayerDataReceivedEventArgs e)
        {
            mReceivedQueue.Enqueue(e.ReceivedMsg);
        }

        //public void SetReceiveQueue(ConcurrentQueue<IMessage> queue)
        //{
        //    mReceivedQueue = queue;
        //}

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

