using System;
using System.Collections.Generic;
using System.Text;

namespace PLCSimPP.Comm.CustomExecption
{
    public class UnitNotFindExectption : Exception
    {
        public string TargetAddr { get; set; }


    }
}
