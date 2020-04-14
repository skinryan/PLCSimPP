using System;
using System.Collections.Generic;
using System.Text;
using PLCSimPP.Comm.Interfaces;

namespace PLCSimPP.Comm.Models
{
    public class MsgCmd : IMessage
    {
        public string Command { get; set; }

        public string Param { get; set; }

        public string UnitAddr { get; set; }

        public int Port { get; set; }

        //public byte[] RawData { get; set; }

        public override string ToString()
        {
            return UnitAddr + "|" + Command + "|" + Param;
        }
    }
}
