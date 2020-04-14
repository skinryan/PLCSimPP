using System;
using System.Threading;
using CommonLib.CommandDispatching.Command;

namespace CommonLib.CommandDispatching.Dispatcher
{
    /// <summary>
    /// CommandDispatcher that executes commands at a specified DateTime.
    /// </summary>
    public class DelayCommandDispatcher : ComparableCommandDispatcher
    {
        public DelayCommandDispatcher(string descriptionArg)
            : base(descriptionArg)
        {
        }

        public bool Enqueue(CommandBase commandArg, DateTime executionDateTimeArg)
        {
            DelayCommand delayCommand = new DelayCommand(commandArg, executionDateTimeArg);
            return Enqueue(delayCommand);
        }

        public bool Enqueue(Action actionArg, DateTime executionDateTimeArg)
        {
            return Enqueue(new ActionCommand(actionArg), executionDateTimeArg);
        }

        protected override void OnNextCommandIsAvailable()
        {
            //Delay for the specified time for the next command.  
            //When woken up, check to see if
            //(1) time has elapsed or (2) another (new) command has been added.
            bool wasWaitInterrupted;
            do
            {
                var delayCommand = (DelayCommand)pCommandList[0];
                int millecsecToWait = delayCommand.NumMillesecToExecution;
                //System.Diagnostics.Trace.WriteLine("Waiting: " + millecsecToWait + " - " + delayCommand.ToString());
                if( millecsecToWait <= 0 )
                {
                    //System.Diagnostics.Trace.WriteLine("Waiting Done: " + delayCommand.ToString());
                    break;
                }
                wasWaitInterrupted = Monitor.Wait(pLock, millecsecToWait);
            } while( wasWaitInterrupted );
        }
    }
}
