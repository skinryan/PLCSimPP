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
    public class Labeler : UnitBase
    {
        private readonly IEventAggregator mEventAggr;

        public override void OnReceivedMsg(string cmd, string content)
        {
            base.OnReceivedMsg(cmd, content);

            if (cmd == LcCmds._0011)
            {
                MoveSample();
            }

            if (cmd == LcCmds._0016)
            {
                var tubeid = content.Substring(0, 15);
                var secTubeid = content.Substring(15, 15);
                mEventAggr.GetEvent<PrintLabelEvent>().Publish(tubeid + secTubeid);
            }
        }

        protected override void OnSampleArrived()
        {
            string param = ParamConst.BCR_2 + CurrentSample.SampleID.PadRight(15);
            this.mSendBehavior.PushMsg(new MsgCmd()
            {
                Command = UnitCmds._1011,
                Param = param,
                Port = this.Port,
                UnitAddr = this.Address
            });
        }

        public Labeler() : base()
        {
            mEventAggr = ServiceLocator.Current.GetInstance<IEventAggregator>();
        }
    }
}
