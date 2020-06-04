using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.PLCSimPP.Comm.Events
{
    public class NotifyRackExchangeEvent : PubSubEvent<RackExchangeParam>
    {
    }

    public class RackExchangeParam
    {
        public string Address { get; set; }
        public string Rack { get; set; }
        public string Shelf { get; set; }
    }
}
