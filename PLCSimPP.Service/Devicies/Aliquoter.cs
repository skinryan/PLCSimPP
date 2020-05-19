using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommonServiceLocator;
using PLCSimPP.Comm;
using PLCSimPP.Comm.Constants;
using PLCSimPP.Comm.Events;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Comm.Models;
using PLCSimPP.Service.Devicies.StandardResponds;
using Prism.Events;

namespace PLCSimPP.Service.Devicies
{
    [Serializable]
    public class Aliquoter : UnitBase
    {
        private const string SERUM = "0041";
        private const string LASTORDER = "FF";
        private IEventAggregator mEventAggr;

        private ISample mSecTube = null;
        private ConcurrentQueue<ISample> mSecQueue;
        private Task mSecTask;


        public override void OnReceivedMsg(string cmd, string content)
        {
            base.OnReceivedMsg(cmd, content);

            if (cmd == LcCmds._0011)
            {
                var bcrNum = content.Substring(0, 1);
                if (bcrNum == ParamConst.BCR_1)
                {
                    SendArrivel();
                }
            }

            if (cmd == LcCmds._0012)
            {
                base.MoveSample();
            }

            if (cmd == LcCmds._0013)
            {
                //reply 1013
                var msg = new MsgCmd()
                {
                    Port = this.Port,
                    UnitAddr = this.Address,
                    Command = UnitCmds._1013
                };
                msg.Param = CurrentSample.SampleID.PadRight(15) + SERUM;
                mSendBehavior.PushMsg(msg);
            }

            if (cmd == LcCmds._0014)
            {
                //reply 1014
                var bcr = content.Substring(0, 1);
                var msg = new MsgCmd()
                {
                    Port = this.Port,
                    UnitAddr = this.Address,
                    Command = UnitCmds._1014
                };
                msg.Param = CurrentSample.SampleID.PadRight(15) + " ";

                mSendBehavior.PushMsg(msg);

                
            }

            if (cmd == LcCmds._0015)
            {
                var seq = content.Substring(19, 2);
                var dest = mRouterService.FindNextDestination(this);
                dest.EnqueueSample(this.mSecTube);
                this.mSecTube = null;

                //must wait all secondary tube is finished,then move primary tube
                if (seq == LASTORDER)
                {
                    MoveSample();
                }
               
            }
        }

        private void SendArrivel()
        {
            var msg = new MsgCmd()
            {
                Port = this.Port,
                UnitAddr = this.Address,
                Command = UnitCmds._1011
            };
            msg.Param = ParamConst.BCR_2 + CurrentSample.SampleID.PadRight(15);

            this.mSendBehavior.PushMsg(msg);
        }

        public override void InitUnit()
        {
            base.InitUnit();
            mSecTask = new Task(ProcessSecQueue);
            mSecTask.Start();

            mEventAggr.GetEvent<PrintLabelEvent>().Subscribe(OnLabelPrinted);
        }

        private void ProcessSecQueue()
        {
            try
            {
                while (true)
                {
                    if (mSecTube == null)
                    {
                        if (this.mSecQueue.TryDequeue(out mSecTube))
                        {
                            //On secondary sample arrived
                            var msg = new MsgCmd()
                            {
                                Port = this.Port,
                                UnitAddr = this.Address,
                                Command = UnitCmds._1011
                            };
                            msg.Param = ParamConst.BCR_3 + mSecTube.SampleID.Trim().PadRight(15);

                            this.mSendBehavior.PushMsg(msg);
                        }
                    }

                    Thread.Sleep(mArrivalInterval);
                }
            }
            catch (System.Exception ex)
            {
                mLogger.LogSys("ProcessSecQueue() error", ex);
            }
        }

        public Aliquoter() : base()
        {
            mEventAggr = ServiceLocator.Current.GetInstance<IEventAggregator>();
            mSecQueue = new ConcurrentQueue<ISample>();
        }

        private void OnLabelPrinted(string tubeid)
        {
            // create secondary tube 
            var id = tubeid.Substring(0, 15);
            var secid = tubeid.Substring(15, 15);

            var subSample = new Sample()
            {
                SampleID = id.Trim() + secid.Trim(),
                DcToken = CurrentSample.DcToken,
                DxCToken = CurrentSample.DxCToken,
                IsSubTube = true
            };

            mSecQueue.Enqueue(subSample);

        }
    }
}
