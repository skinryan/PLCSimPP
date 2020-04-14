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
    public class HMOutlet : UnitBase
    {
        public override void OnReceivedMsg(string cmd, string content)
        {
            base.OnReceivedMsg(cmd, content);

            if (cmd == LcCmds._0011)
            {
                
                base.MoveSample();
            }

            //if (cmd == LcCmds._0012)
            //{
                
            //}
        }

      

        public HMOutlet() : base()
        {

        }
    }
}
