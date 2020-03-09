using System;
using System.Collections.Generic;
using System.Text;

namespace PLCSimPP.Comm.Models
{
    public class MsgLog
    {
        public long ID { get; set; }

        public DateTime Time { get; set; }

        public string Direction { get; set; }

        public string Address { get; set; }

        public string Command { get; set; }

        public string Details { get; set; }

        public string Token { get; set; }

    }
}
