using System;
using System.Collections.Generic;
using System.Text;
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
        private readonly IEventAggregator mEventAggr;

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
                var tubid = content.Substring(0, 15);
                var seq = content.Substring(21, 2);
                var sample = new Sample()
                {
                    SampleID = tubid.Trim(),
                };

                var dest = mRouterService.FindNextDestination(this);
                dest.EnqueueSample(sample);

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


        public Aliquoter() : base()
        {
            mEventAggr = ServiceLocator.Current.GetInstance<IEventAggregator>();
            mEventAggr.GetEvent<PrintLabelEvent>().Subscribe(OnLabelPrinted);
        }

        private void OnLabelPrinted(string tubeid)
        {
            // send 1011 for sec tube
            var id = tubeid.Substring(0, 15);
            var secid = tubeid.Substring(15, 15);

            var msg = new MsgCmd()
            {
                Port = this.Port,
                UnitAddr = this.Address,
                Command = UnitCmds._1011
            };
            msg.Param = ParamConst.BCR_3 + (id.Trim() + secid.Trim()).PadRight(15);

            this.mSendBehavior.PushMsg(msg);
        }
    }
}
