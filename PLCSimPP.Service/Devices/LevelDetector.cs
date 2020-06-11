using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using BCI.PLCSimPP.Comm;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Models;
using BCI.PLCSimPP.Service.Devices.StandardResponds;

namespace BCI.PLCSimPP.Service.Devices
{
    [Serializable]
    public class LevelDetector : UnitBase
    {
        const int CLOT_TIME_OUT = 20;
        private List<ISample> mClotedSamples;
        private Timer mClotTimer;
        private int mClotTime;
        private object mlocker;

        protected override int GetPendingCount()
        {
            return mClotedSamples.Count + mPendingQueue.Count;
        }

        public override void OnReceivedMsg(string cmd, string content)
        {
            base.OnReceivedMsg(cmd, content);

            if (cmd == LcCmds._0011)
            {
                mClotedSamples.Add(CurrentSample);
                CurrentSample = null;
                RaisePropertyChanged("PendingCount");

                if (mClotedSamples.Count == 5)
                {
                    Reply_1012();
                    mClotTime = 0;
                }
            }

            if (cmd == LcCmds._0012)
            {
                base.MoveSample();
            }
        }

        private void Reply_1012()
        {
            lock (mlocker)
            {
                if (mClotedSamples.Count > 0)
                {
                    //reply 1012
                    MsgCmd msg = new MsgCmd();
                    msg.Command = UnitCmds._1012;
                    msg.Port = this.Port;
                    msg.UnitAddr = this.Address;

                    msg.Param = mClotedSamples.Count.ToString();
                    foreach (var sample in mClotedSamples)
                        msg.Param += sample.SampleID.PadRight(15) + "0000";

                    mSendBehavior.PushMsg(msg);

                    MoveDetectedSample();

                    mClotedSamples.Clear();

                    RaisePropertyChanged("PendingCount");
                }
            }
        }

        private void MoveDetectedSample()
        {
            var dest = mRouterService.FindNextDestination(this);

            foreach (var sample in mClotedSamples)
            {
                dest.EnqueueSample(sample);                
            }
        }

        private void ProcessClot(object state)
        {
            if (mClotedSamples.Count <= 0)
            {
                return;
            }

            if (mClotTime >= CLOT_TIME_OUT)
            {
                Reply_1012();
                mClotTime = 0;
            }

            mClotTime += 1;
        }

        public LevelDetector() : base()
        {
            mlocker = new object();
            mClotedSamples = new List<ISample>();
            mClotTimer = new Timer(ProcessClot, null, 1000, 1000);

        }
    }
}
