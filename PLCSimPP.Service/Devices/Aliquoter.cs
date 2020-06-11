using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommonServiceLocator;
using BCI.PLCSimPP.Comm;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Events;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Models;
using BCI.PLCSimPP.Service.Devices.StandardResponds;
using Prism.Events;

namespace BCI.PLCSimPP.Service.Devices
{
    [Serializable]
    public class Aliquoter : UnitBase
    {
        private const string SERUM = "0041";// Simulate based on the production environment log
        private const string LAST_ORDER = "FF";

        private ISample mSecTube = null;
        private ConcurrentQueue<ISample> mSecQueue;
        private Task mSecTask;

        public override void OnReceivedMsg(string cmd, string content)
        {
            base.OnReceivedMsg(cmd, content);

            if (cmd == LcCmds._0001)
            {
                //simulate 1017               
                var msg1017 = new MsgCmd()
                {
                    Port = this.Port,
                    UnitAddr = this.Address,
                    Command = UnitCmds._1017,
                    Param = "10"//Simulate based on the production environment log,
                                //the first character represents Sensor position,
                                //second character 0': Holder queue cleared, '1': Holder queue occurred
                };
                base.mSendBehavior.PushMsg(msg1017);
            }

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
                var msg = new MsgCmd
                {
                    Port = this.Port,
                    UnitAddr = this.Address,
                    Command = UnitCmds._1013,
                    Param = CurrentSample.SampleID.PadRight(15) + SERUM
                };
                mSendBehavior.PushMsg(msg);
            }

            if (cmd == LcCmds._0014)
            {
                //reply 1014
                var bcr = content.Substring(0, 1);
                var msg = new MsgCmd
                {
                    Port = this.Port,
                    UnitAddr = this.Address,
                    Command = UnitCmds._1014,
                    Param = CurrentSample.SampleID.PadRight(15) + " "
                };

                mSendBehavior.PushMsg(msg);
            }

            if (cmd == LcCmds._0015)
            {
                var seq = content.Substring(19, 2);
                var dest = mRouterService.FindNextDestination(this);
                dest.EnqueueSample(this.mSecTube);
                this.mSecTube = null;

                //must wait all secondary tube is finished,then move primary tube
                if (seq == LAST_ORDER)
                {
                    MoveSample();
                }
            }
        }

        private void SendArrivel()
        {
            var msg = new MsgCmd
            {
                Port = this.Port,
                UnitAddr = this.Address,
                Command = UnitCmds._1011,
                Param = ParamConst.BCR_2 + CurrentSample.SampleID.PadRight(15)
            };

            this.mSendBehavior.PushMsg(msg);
        }

        public override void InitUnit()
        {
            base.InitUnit();
            mSecTask = new Task(ProcessSecQueue);
            mSecTask.Start();

            mEventAggr.GetEvent<PrintLabelEvent>().Subscribe(OnLabelPrinted);
        }

        /// <summary>
        /// secondary tube arrival processor
        /// </summary>
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
                            var msg = new MsgCmd
                            {
                                Port = this.Port,
                                UnitAddr = this.Address,
                                Command = UnitCmds._1011,
                                Param = ParamConst.BCR_3 + mSecTube.SampleID.Trim().PadRight(15)
                            };

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
            mSecQueue = new ConcurrentQueue<ISample>();
        }

        private void OnLabelPrinted(string tubeId)
        {
            // create secondary tube 
            var id = tubeId.Substring(0, 15);
            var secId = tubeId.Substring(15, 15);

            var subSample = new Sample()
            {
                SampleID = id.Trim() + secId.Trim(),
                DcToken = CurrentSample.DcToken,
                DxCToken = CurrentSample.DxCToken,
                IsSubTube = true
            };

            mSecQueue.Enqueue(subSample);
        }
    }
}
