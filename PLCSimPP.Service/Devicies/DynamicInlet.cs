using System;
using System.Collections.Generic;
using System.Text;
using PLCSimPP.Comm;
using PLCSimPP.Comm.Constants;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Comm.Models;
using PLCSimPP.Service.Devicies.StandardResponds;

namespace PLCSimPP.Service.Devicies
{
    [Serializable]
    public class DynamicInlet : UnitBase
    {
        public override void OnReceivedMsg(string cmd, string content)
        {
            base.OnReceivedMsg(cmd, content);

            if (cmd == LcCmds._0011)
            {
                //todo replay 1015
            }

            if (cmd == LcCmds._0012)
            {
                base.MoveSample();
            }
        }



        protected override void OnSampleArrived()
        {
            var msg = SendMsg.GetMsg_1024(this);
            this.mSendBehavior.PushMsg(msg);
        }

        public DynamicInlet() : base()
        {

        }
    }
}
