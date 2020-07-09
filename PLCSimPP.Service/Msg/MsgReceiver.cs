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
    public class MsgReceiver : IRecvMsgBehavior
    {
        private int mReceiveInterval = 100;

        private bool mWorking;//is working flag
        private bool mCanActive;//can active flag
        private string mToken;//script token
        private Task mRecvTask;//receive msg task  
        private readonly IRouterService mRouterService;
        private readonly ILogService mLogger;
        private readonly IEventAggregator mEventAggr;
        private readonly IPortService mPortService;

        /// <summary>
        /// Active Receive message Task
        /// </summary>
        /// <param name="token"></param>
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

        /// <summary>
        /// Receive message Sequence
        /// </summary>
        private void RecvSequence()
        {
            while (true)
            {
                Thread.Sleep(mReceiveInterval);

                try
                {
                    var flag = mPortService.TryDequeue(out IMessage result);

                    if (flag)
                    {
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

        /// <summary>
        /// Stop Receive message Task
        /// </summary>
        public void StopRecvTask()
        {
            mWorking = false;
            mToken = string.Empty;
            mRecvTask.ContinueWith((task) =>
            {
                mCanActive = true;
            });
        }


        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="eventAggr"></param>
        /// <param name="logger"></param>
        /// <param name="msgService"></param>
        /// <param name="router"></param>
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
