using System.Collections.Concurrent;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Interfaces.Services;
using BCI.PLCSimPP.Service.Services;
using Prism.Events;

namespace BCI.PLCSimPP.Service.Msg
{
    public class PortService : IPortService
    {
        private ConcurrentQueue<IMessage> mReceivedQueue;

        public PortConnection MasterPort1 { get; set; }
        public PortConnection MasterPort2 { get; set; }
        public PortConnection MasterPort3 { get; set; }

        public PortService()
        {
            mReceivedQueue = new ConcurrentQueue<IMessage>();
        }

        public void Connect()
        {
            MasterPort1.OpenConnect();
            MasterPort2.OpenConnect();
            MasterPort3.OpenConnect();
        }

        public void Disconnect()
        {
            MasterPort1.CloseConnect();
            MasterPort2.CloseConnect();
            MasterPort3.CloseConnect();
            mReceivedQueue = new ConcurrentQueue<IMessage>();
        }

        public PortService(ILogService logger, IEventAggregator aggregator)
        {
            var logger1 = logger;
            var eventAggr = aggregator;

            MasterPort1 = new PortConnection(1, logger1, eventAggr);
            MasterPort2 = new PortConnection(2, logger1, eventAggr);
            MasterPort3 = new PortConnection(3, logger1, eventAggr);

            MasterPort1.OnMsgReceived += MasterPort_OnMsgReceived;
            MasterPort2.OnMsgReceived += MasterPort_OnMsgReceived;
            MasterPort3.OnMsgReceived += MasterPort_OnMsgReceived;
        }

        private void MasterPort_OnMsgReceived(object sender, Communication.EventArguments.TransportLayerDataReceivedEventArgs e)
        {
            mReceivedQueue.Enqueue(e.ReceivedMsg);
        }


        public void SendMsg(IMessage msg)
        {
            if (msg.Port == 1)
            {
                MasterPort1.SendMsg(msg);
            }
            else if (msg.Port == 2)
            {
                MasterPort2.SendMsg(msg);
            }
            else if (msg.Port == 3)
            {
                MasterPort3.SendMsg(msg);
            }
        }

        public bool TryDequeue(out IMessage msg)
        {
            return mReceivedQueue.TryDequeue(out msg);
        }
    }
}

