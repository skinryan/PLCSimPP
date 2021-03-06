﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCI.PLCSimPP.Comm.Constants;

namespace BCI.PLCSimPP.Service.Devices
{
    [Serializable]
    public class ErrorLane : UnitBase
    {
        /// <inheritdoc />
        public override void OnReceivedMsg(string cmd, string content)
        {
            base.OnReceivedMsg(cmd, content);

            if (cmd == LcCmds._0011)
            {
                base.MoveSample();
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        public ErrorLane() : base()
        {

        }
    }
}
