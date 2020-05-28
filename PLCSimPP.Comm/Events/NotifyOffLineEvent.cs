using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.PLCSimPP.Comm.Events
{
    /// <summary>
    /// sample on/of line notification 
    /// param 1:online, -1:offline, 0:reset
    /// </summary>
    public class NotifyOnlineSampleEvent : PubSubEvent<int>
    {
    }
}
