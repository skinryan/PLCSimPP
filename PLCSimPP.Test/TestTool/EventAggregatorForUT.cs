using System;
using System.Collections.Generic;
using System.Text;
using Prism.Events;

namespace BCI.PLCSimPP.Test.TestTool
{
    public class EventAggregatorForUT : IEventAggregator
    {
        public TEventType GetEvent<TEventType>() where TEventType : EventBase, new()
        {
            var type = typeof(TEventType);

            return (TEventType)System.Activator.CreateInstance(type);
        }
    }
}
