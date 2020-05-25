using System;
using System.Collections.Generic;
using System.Text;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Communication.Models;

namespace BCI.PLCSimPP.Communication.EventArguments
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
