using System;
using System.Collections.Generic;
using System.Text;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Communication.Models;

namespace PLCSimPP.Communication.EventArguments
{
    public class SmartConnectionDataReceivedEventArgs : EventArgs
    {
        public SmartConnectionDataReceivedEventArgs(IMessage msg)
        {
            Msg = msg;
        }

        public IMessage Msg
        {
            get;
            private set;
        }
    }
}
