using System;
using System.Collections.Generic;
using System.Text;

namespace PLCSimPP.Communication.Models
{
    public class CmdMsg
    {
        public string Command { get; set; }

        public string Param { get; set; }

        public string Unit { get; set; }

        //public byte[] RawData { get; set; }

        public override string ToString()
        {
            return Unit + "|" + Command + "|" + Param;
        }
    }
}
