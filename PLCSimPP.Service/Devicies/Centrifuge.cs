using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PLCSimPP.Comm;
using PLCSimPP.Comm.Constants;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Comm.Models;
using PLCSimPP.Service.Devicies.StandardResponds;

namespace PLCSimPP.Service.Devicies
{
    [Serializable]
    public class Centrifuge : UnitBase
    {
        const int CENTRIFUGE_MAX_CAPACITY = 40;
        const int SPINNING_TIME = 2000;
        private int mSecCount;// time count
        private bool mSpinning;// centrifuge working flag
        private readonly Timer mSpinningTimer;
        private List<ISample> StoredSamples { get; set; }

        /// <summary>
        /// if centrifuge is full ,return false
        /// </summary>
        public bool CanSortingSample
        {
            get
            {
                if (!mSpinning)
                {
                    return StoredSamples.Count < CENTRIFUGE_MAX_CAPACITY;
                }
                else
                {
                    return false;
                }
            }
        }

        public override void OnReceivedMsg(string cmd, string content)
        {
            base.OnReceivedMsg(cmd, content);

            if (cmd == LcCmds._0011)
            {
                StoreSample();
            }

            if (cmd == LcCmds._0012)
            {
                base.MoveSample();
            }

            if (cmd == LcCmds._001A)
            {
                MoveCentrifugedSampleToOutQueue();
            }
        }

        private void MoveCentrifugedSampleToOutQueue()
        {
            foreach (var sample in StoredSamples)
            {
                this.MoveSampleToNext(sample);
            }

            IMessage notifyUnloading = new MsgCmd()
            {
                Command = UnitCmds._1022,
                Param = ParamConst.CENT_STATUS_UNLOADING,
                Port = this.Port,
                UnitAddr = this.Address
            };
            base.mSendBehavior.PushMsg(notifyUnloading);

            mSpinning = false;
        }

        private void MoveSampleToNext(ISample sample)
        {
            var dest = mRouterService.FindNextDestination(this);
            dest.EnqueueSample(sample);
        }

        public override void InitUnit()
        {
            base.InitUnit();

            mWaitArrivalTask = new Task(ProcessInSample);
            mWaitArrivalTask.Start();

            //mSpinningTimer.Start();
        }

        private void StoreSample()
        {
            StoredSamples.Add(CurrentSample);

            CurrentSample = null;
        }

        private void ProcessInSample()
        {
            try
            {
                while (true)
                {
                    if (CurrentSample == null)
                    {
                        if (CanSortingSample)
                        {
                            if (this.TryDequeueSample(out mCurrentSample))
                            {
                                OnSampleArrived();
                            }
                        }
                    }

                    Thread.Sleep(mArrivalInterval);
                }
            }
            catch (System.Exception ex)
            {
                mLogger.LogSys("ProcessPendingQueue() error", ex);
            }
        }

        private void ProcessSpinning(object state)
        {

            //while (true)
            //{
            if (!CanSortingSample && !mSpinning)
            {
                mSpinning = true;

                IMessage notifyStart = new MsgCmd()
                {
                    Command = UnitCmds._1022,
                    Param = ParamConst.CENT_STATUS_START,
                    Port = this.Port,
                    UnitAddr = this.Address
                };

                base.mSendBehavior.PushMsg(notifyStart);

                Thread.Sleep(SPINNING_TIME);//Simulate centrifuge operation time

                IMessage notifyEnd = new MsgCmd()
                {
                    Command = UnitCmds._1022,
                    Param = ParamConst.CENT_STATUS_END,
                    Port = this.Port,
                    UnitAddr = this.Address
                };
                base.mSendBehavior.PushMsg(notifyEnd);

                IMessage querySorting = new MsgCmd()
                {
                    Command = UnitCmds._101A,
                    Param = "1",
                    Port = this.Port,
                    UnitAddr = this.Address
                };
                base.mSendBehavior.PushMsg(querySorting);
                //mSpinning = false;
            }

            //    Thread.Sleep(1000);
            //}
        }



        public Centrifuge() : base()
        {
            StoredSamples = new List<ISample>();
            mSpinningTimer = new Timer(ProcessSpinning, null, 1000, 1000);

        }
    }


}
