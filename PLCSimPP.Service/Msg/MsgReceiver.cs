using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Interfaces.Services;
using BCI.PLCSimPP.Comm.Models;
using BCI.PLCSimPP.Service.Router;
using Prism.Events;

namespace BCI.PLCSimPP.Service.Msg
{
    public class MsgReceiver : IRecvMsgBeheavior
    {
        private int mReceiveInterval = 100;

        private bool mWorking;//is working flag
        private bool mCanActive;//can active flag
        private string mToken;//script token
        private Task mRecvTask;//receive msg task  
        private IRouterService mRouterService;

        //private readonly ConcurrentQueue<IMessage> mReceiveQueue;// receive msg queue
        private readonly ILogService mLogger;
        private readonly IEventAggregator mEventAggr;
        private readonly IPortService mPortService;

        public void ActiveRecvTask(string token)
        {
            if (!mCanActive)
                return;

            mToken = token;

            if (mRecvTask.IsCompleted)
            {
                mRecvTask = new Task(RecvSequence);
            }

            mWorking = true;
            mRecvTask.Start();
            mCanActive = false;
        }

        private void RecvSequence()
        {
            //mMsgService.SetReceiveQueue(mReceiveQueue);

            while (true)
            {
                Thread.Sleep(mReceiveInterval);

                try
                {
                    var flag = mPortService.TryDequeue(out IMessage result);

                    if (flag)
                    {
                        //var success = mMsgService.TryDequeue(out IMessage result);


                        //log msg
                        mLogger.LogRecvMsg(result.UnitAddr, result.Command, result.Param);

                        //add to monitor
                        mEventAggr.GetEvent<Comm.Events.MonitorEvent>().Publish(
                            new Comm.Models.MsgLog()
                            {
                                Time = DateTime.Now,
                                Address = result.UnitAddr,
                                Command = result.Command,
                                Details = result.Param,
                                Direction = Comm.Constants.CommConst.RECV
                            });

                        var units = mRouterService.FindTargetUnit(result.UnitAddr);
                        foreach (var unit in units)
                        {
                            unit.OnReceivedMsg(result.Command, result.Param);
                        }

                    }
                }
                catch (System.Exception ex)
                {
                    mLogger.LogSys(ex.Message, ex);
                }
            }
        }

        public void StopRecvTask()
        {
            mWorking = false;
            mToken = string.Empty;
            mRecvTask.ContinueWith((task) =>
            {
                mCanActive = true;
            });
            //mUnitCollection = null;
        }

        //public void PushReceivedMsg(IMessage msg)
        //{
        //    this.mReceiveQueue.Enqueue(msg);
        //}

        //public void SetUnitCollection(ObservableCollection<IUnit> unitCollection)
        //{
        //    this.mUnitCollection = unitCollection;
        //}

        public MsgReceiver(IEventAggregator eventAggr, ILogService logger, IPortService msgService, IRouterService router)
        {
            mEventAggr = eventAggr;
            mLogger = logger;
            mPortService = msgService;
            mRouterService = router;

            mRecvTask = new Task(RecvSequence);
            mCanActive = true;
        }
    }
}
