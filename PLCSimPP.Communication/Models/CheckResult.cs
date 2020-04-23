using System;
using System.Collections.Generic;
using System.Text;

namespace PLCSimPP.Communication.Models
{
    public enum ResultType
    {
        Heartbeat,
        RawData,
        InvalidLength,
        InvalidCmd,
        Confirm
    }

    public class CheckResult
    {
        public ResultType Result { get; set; }

        public byte[] Content { get; set; }

        public string InvalidCommand { get; set; }

        public int ExpectLength { get; set; }

        public int ActualLength { get; set; }
    }
}
