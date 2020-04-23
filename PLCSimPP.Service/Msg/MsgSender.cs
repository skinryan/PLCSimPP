using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommonServiceLocator;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Comm.Interfaces.Services;
using Prism.Events;

namespace PLCSimPP.Service.Msg
{
    public class MsgSender : ISendMsgBehavior
    {
        private int mSendInterval;
        private bool mWorking;//is working flag
        private bool mCanActive;//can active flag
        private string mToken;//script token
        private Task mSendTask;//send msg task  

        private readonly ConcurrentQueue<IMessage> mSendQueue;//send message queue
        private readonly ILogService mLogger;
        private readonly IRecvMsgBeheavior mMsgReceiver;
        private readonly IConfigService mConfig;
        private readonly IMsgService mMsgService;
        private readonly IEventAggregator mEventAggr;

        public bool IsPaused { get; set; } = false;

        public MsgSender(IMsgService msgService, ILogService logger, IConfigService config, IEventAggregator eventAggr)
        {
            mEventAggr = eventAggr;
            mMsgService = msgService;
            mLogger = logger;
            mConfig = config;

            mSendInterval = 50;//config.ReadSysConfig().MsgSendInterval;
            mSendQueue = new ConcurrentQueue<IMessage>();
            mSendTask = new Task(SendSequence);
            mCanActive = true;
        }

        public void ActiveSendTask(string token)
        {
            if (!mCanActive)
                return;

            mToken = token;

            if (mSendTask.IsCompleted)
            {
                mSendTask = new Task(SendSequence);
            }

            mWorking = true;
            mSendTask.Start();
            mCanActive = false;
        }

        /// <summary>
        /// Send message sequence
        /// </summary>
        private void SendSequence()
        {
            while (mWorking)
            {
                Thread.Sleep(mSendInterval);
                if (IsPaused)
                    continue;

                try
                {
                    if (mSendQueue.Count > 0)
                    {
                        bool deqflag = mSendQueue.TryDequeue(out var msg);
                        if (deqflag)
                        {
                            mMsgService.SendMsg(msg);
                                                        
                            mLogger.LogSendMsg(msg.UnitAddr, msg.Command, msg.Param);

                            //add to monitor
                            mEventAggr.GetEvent<Comm.Events.MonitorEvent>().Publish(
                                new Comm.Models.MsgLog()
                                {
                                    Time = DateTime.Now,
                                    Address = msg.UnitAddr,
                                    Command = msg.Command,
                                    Details = msg.Param,
                                    Direction = Comm.Constants.CommConst.SEND
                                });
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    mLogger.LogSys(ex.Message, ex);
                }
            }
        }

        public void PushMsg(IMessage msg)
        {
            mSendQueue.Enqueue(msg);
        }

        public void StopSendTask()
        {
            mWorking = false;
            mToken = string.Empty;
            mSendTask.ContinueWith((task) =>
            {
                mCanActive = true;
            });
        }
    }
}
