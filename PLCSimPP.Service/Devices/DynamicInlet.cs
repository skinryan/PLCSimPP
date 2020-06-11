using System;
using System.Collections.Generic;
using System.Text;
using BCI.PLCSimPP.Comm;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Models;
using BCI.PLCSimPP.Service.Devices.StandardResponds;

namespace BCI.PLCSimPP.Service.Devices
{
    [Serializable]
    public class DynamicInlet : UnitBase
    {
        public override void OnReceivedMsg(string cmd, string content)
        {
            base.OnReceivedMsg(cmd, content);
            
            if (cmd == LcCmds._0012)
            {
                base.MoveSample();
            }
        }
               
        protected override void OnSampleArrived()
        {
            var msg = SendMsg.GetMsg1024(this);
            this.mSendBehavior.PushMsg(msg);
        }

        public DynamicInlet() : base()
        {

        }
    }
}
