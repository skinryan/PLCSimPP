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
        private object mLocker;

        /// <summary>
        /// get padding queue count
        /// </summary>
        /// <returns></returns>
        protected override int GetPendingCount()
        {
            return mClotedSamples.Count + mPendingQueue.Count;
        }

        /// <inheritdoc />
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
                    Reply1012();
                    mClotTime = 0;
                }
            }

            if (cmd == LcCmds._0012)
            {
                base.MoveSample();
            }
        }

        /// <summary>
        /// replay 1012 command
        /// </summary>
        private void Reply1012()
        {
            lock (mLocker)
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

                    MoveDetectedSample();//movie all sample

                    mClotedSamples.Clear();

                    RaisePropertyChanged("PendingCount");
                }
            }
        }

        /// <summary>
        /// move sample to next unit
        /// </summary>
        private void MoveDetectedSample()
        {
            var dest = mRouterService.FindNextDestination(this);

            foreach (var sample in mClotedSamples)
            {
                dest.EnqueueSample(sample);                
            }
        }

        /// <summary>
        /// Process Clot operate
        /// </summary>
        /// <param name="state"></param>
        private void ProcessClot(object state)
        {
            if (mClotedSamples.Count <= 0)
            {
                return;
            }

            if (mClotTime >= CLOT_TIME_OUT)
            {
                Reply1012();
                mClotTime = 0;
            }

            mClotTime += 1;
        }

        /// <summary>
        /// constructor
        /// </summary>
        public LevelDetector() : base()
        {
            mLocker = new object();
            mClotedSamples = new List<ISample>();
            mClotTimer = new Timer(ProcessClot, null, 1000, 1000);
        }
    }
}
