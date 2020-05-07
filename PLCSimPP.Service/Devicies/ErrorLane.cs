using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLCSimPP.Comm.Constants;

namespace PLCSimPP.Service.Devicies
{
    [Serializable]
    public class ErrorLane : UnitBase
    {
        public override void OnReceivedMsg(string cmd, string content)
        {
            base.OnReceivedMsg(cmd, content);

            if (cmd == LcCmds._0011)
            {
                base.MoveSample();
            }
        }

        public ErrorLane()
        {
        }


    }
}
