using System;
using System.Collections.Generic;
using System.Text;

namespace BCI.PLCSimPP.Comm.CustomExecption
{
    public class UnitNotFindExectption : Exception
    {
        public string TargetAddr { get; set; }


    }
}
