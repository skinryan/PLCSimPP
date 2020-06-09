using System;
using System.Collections.Generic;
using System.Text;

namespace BCI.PLCSimPP.Comm.CustomException
{
    public class UnitNotFindException : Exception
    {
        public string TargetAddr { get; set; }


    }
}
