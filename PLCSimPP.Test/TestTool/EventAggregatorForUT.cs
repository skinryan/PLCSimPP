using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Prism.Events;

namespace BCI.PLCSimPP.Test.TestTool
{
    public class EventAggregatorForUT : IEventAggregator
    {
        private readonly Dictionary<Type, EventBase> events = new Dictionary<Type, EventBase>();
        // Captures the sync context for the UI thread when constructed on the UI thread 
        // in a platform agnositc way so it can be used for UI thread dispatching
        private readonly SynchronizationContext syncContext = SynchronizationContext.Current;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public TEventType GetEvent<TEventType>() where TEventType : EventBase, new()
        {
            lock (events)
            {
                EventBase existingEvent = null;

                if (!events.TryGetValue(typeof(TEventType), out existingEvent))
                {
                    TEventType newEvent = new TEventType();
                    newEvent.SynchronizationContext = syncContext;
                    events[typeof(TEventType)] = newEvent;

                    return newEvent;
                }
                else
                {
                    return (TEventType)existingEvent;
                }
            }
        }
    }
}
