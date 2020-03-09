using System;
using System.Collections.Generic;
using System.Text;
using PLCSimPP.Communication.Models;

namespace PLCSimPP.Communication.EventArguments
{
    public class SmartConnectionDataReceivedEventArgs : EventArgs
    {
        public SmartConnectionDataReceivedEventArgs(CmdMsg msg)
        {
            Msg = msg;
        }

        public CmdMsg Msg
        {
            get;
            private set;
        }
    }
}
