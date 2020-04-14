using System;
using CommonLib.CommandDispatching.Command;

namespace CommonLib.CommandDispatching.Dispatcher
{
    /// <summary>
    /// CommandDispatcher that executes commands in priority order with an optional DateTime.
    /// </summary>
    public class PriorityWithDelayCommandDispatcher
    {
        private readonly DelayCommandDispatcher mDelayCommandDispatcher;
        private readonly PriorityCommandDispatcher mPriorityCommandDispatcher;

        public PriorityWithDelayCommandDispatcher(string descriptionArg)
        {
            mDelayCommandDispatcher = new DelayCommandDispatcher(descriptionArg);
            mPriorityCommandDispatcher = new PriorityCommandDispatcher(descriptionArg);
        }

        public void Enqueue(CommandBase commandArg)
        {
            Enqueue(commandArg, PriorityCommandDispatcher.Priority.Normal);
        }
        public void Enqueue(Action actionArg)
        {
            Enqueue(actionArg, PriorityCommandDispatcher.Priority.Normal);
        }

        public void Enqueue(Action actionArg, int priorityArg, uint numMillesecToWaitBeforeCommandExecutionArg)
        {
            Enqueue(actionArg, priorityArg, ToExecutionDateTime(numMillesecToWaitBeforeCommandExecutionArg));
        }
        public void Enqueue(Action actionArg, PriorityCommandDispatcher.Priority priorityArg, uint numMillesecToWaitBeforeCommandExecutionArg)
        {
            Enqueue(actionArg, (int)priorityArg, numMillesecToWaitBeforeCommandExecutionArg);
        }

        public void Enqueue(Action actionArg, int priorityArg, DateTime executionDateTimeArg)
        {
            ActionCommand transferToPriorityQueueCommand = new ActionCommand(() => EnqueuePriorityCommand(actionArg, priorityArg));
            EnqueueDelayCommand(transferToPriorityQueueCommand, executionDateTimeArg);
        }
        public void Enqueue(Action actionArg, PriorityCommandDispatcher.Priority priorityArg, DateTime executionDateTimeArg)
        {
            Enqueue(actionArg, (int)priorityArg, executionDateTimeArg);
        }

        public void Enqueue(CommandBase commandArg, int priorityArg, uint numMillesecToWaitBeforeCommandExecutionArg)
        {
            Enqueue(commandArg, priorityArg, ToExecutionDateTime(numMillesecToWaitBeforeCommandExecutionArg));
        }
        public void Enqueue(CommandBase commandArg, PriorityCommandDispatcher.Priority priorityArg, uint numMillesecToWaitBeforeCommandExecutionArg)
        {
            Enqueue(commandArg, (int)priorityArg, numMillesecToWaitBeforeCommandExecutionArg);
        }
        public void Enqueue(CommandBase commandArg, int priorityArg, DateTime executionDateTimeArg)
        {
            ActionCommand transferToPriorityQueueCommand = new ActionCommand(() => EnqueuePriorityCommand(commandArg, priorityArg));
            EnqueueDelayCommand(transferToPriorityQueueCommand, executionDateTimeArg);
        }
        public void Enqueue(CommandBase commandArg, PriorityCommandDispatcher.Priority priorityArg, DateTime executionDateTimeArg)
        {
            Enqueue(commandArg, (int)priorityArg, executionDateTimeArg);
        }

        public void Enqueue(CommandBase commandArg, int priorityArg)
        {
            EnqueuePriorityCommand(commandArg, priorityArg);
        }
        public void Enqueue(CommandBase commandArg, PriorityCommandDispatcher.Priority priorityArg)
        {
            Enqueue(commandArg, (int)priorityArg);
        }
        public void Enqueue(Action actionArg, int priorityArg)
        {
            EnqueuePriorityCommand(actionArg, priorityArg);
        }
        public void Enqueue(Action actionArg, PriorityCommandDispatcher.Priority priorityArg)
        {
            Enqueue(actionArg, (int)priorityArg);
        }

        private void EnqueuePriorityCommand(CommandBase commandArg, int priorityArg)
        {
            mPriorityCommandDispatcher.Enqueue(commandArg, priorityArg);
        }
        private void EnqueuePriorityCommand(Action actionArg, int priorityArg)
        {
            mPriorityCommandDispatcher.Enqueue(actionArg, priorityArg);
        }

        private void EnqueueDelayCommand(ActionCommand transferToPriorityQueueCommand, DateTime executionDateTimeArg)
        {
            mDelayCommandDispatcher.Enqueue(transferToPriorityQueueCommand, executionDateTimeArg);
        }

        private static DateTime ToExecutionDateTime(uint numMillesecToWaitBeforeCommandExecutionArg)
        {
            return DateTime.Now.AddMilliseconds((int)numMillesecToWaitBeforeCommandExecutionArg);
        }

        internal int CommandCount
        {
            get
            {
                //Note: This may not return the exact count since a command may be
                //      in the midst of being transferred from the DelayCommandDispatcher
                //      to the PriorityCommandDispatcher.
                return mDelayCommandDispatcher.CommandCount + mPriorityCommandDispatcher.CommandCount;
            }
        }

        public bool CanStop()
        {
            return mDelayCommandDispatcher.CanStop() && mPriorityCommandDispatcher.CanStop();
        }

        public void Stop()
        {
            mDelayCommandDispatcher.Stop();
            mPriorityCommandDispatcher.Stop();
        }
    }
}
