using System;
using System.Collections.Generic;
using System.Text;
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
    public class Labeler : UnitBase
    {
       
        public override void OnReceivedMsg(string cmd, string content)
        {
            base.OnReceivedMsg(cmd, content);

            if (cmd == LcCmds._0011)
            {
                MoveSample();
            }
            if (cmd == LcCmds._0012)
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
            
        }
    }
}
