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
        /// <summary>
        /// Master Port1
        /// </summary>
        public PortConnection MasterPort1 { get; set; }
        /// <summary>
        /// MasterPort2
        /// </summary>
        public PortConnection MasterPort2 { get; set; }
        /// <summary>
        /// MasterPort3
        /// </summary>
        public PortConnection MasterPort3 { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        public PortService()
        {
            mReceivedQueue = new ConcurrentQueue<IMessage>();
        }

        /// <summary>
        /// Connect all port
        /// </summary>
        public void Connect()
        {
            MasterPort1.OpenConnect();
            MasterPort2.OpenConnect();
            MasterPort3.OpenConnect();
        }

        /// <summary>
        /// disconnect all port and reset receive message queue
        /// </summary>
        public void Disconnect()
        {
            MasterPort1.CloseConnect();
            MasterPort2.CloseConnect();
            MasterPort3.CloseConnect();
            mReceivedQueue = new ConcurrentQueue<IMessage>();
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="aggregator"></param>
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

        /// <summary>
        ///  Message Received event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MasterPort_OnMsgReceived(object sender, Communication.EventArguments.TransportLayerDataReceivedEventArgs e)
        {
            mReceivedQueue.Enqueue(e.ReceivedMsg);
        }

        /// <summary>
        /// Send message to target port 
        /// </summary>
        /// <param name="msg"></param>
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

        /// <summary>
        /// get message from receive queue
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool TryDequeue(out IMessage msg)
        {
            return mReceivedQueue.TryDequeue(out msg);
        }
    }
}

