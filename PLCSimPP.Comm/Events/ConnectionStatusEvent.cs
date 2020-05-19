using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSimPP.Comm.Events
{
    public class ConnectionStatusEvent : PubSubEvent<ConnInfo>
    {

    }

    public class ConnInfo
    {

        public int Port { get; set; }

        public bool IsConnected { get; set; }

    }
}
