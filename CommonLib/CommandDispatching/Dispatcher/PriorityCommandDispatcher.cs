using System;
using CommonLib.CommandDispatching.Command;

namespace CommonLib.CommandDispatching.Dispatcher
{
    /// <summary>
    /// CommandDispatcher that executes commands in priority order.  When the commands are
    /// submitted, a priority is also specified.
    /// </summary>
    public class PriorityCommandDispatcher : ComparableCommandDispatcher
    {
        public enum Priority
        {
            Lowest = int.MaxValue,
            BelowNormal = int.MaxValue / 2,
            Normal = 0,
            AboveNormal = int.MinValue / 2,
            Highest = int.MinValue
        }

        public PriorityCommandDispatcher(string descriptionArg)
            : base(descriptionArg)
        {
        }

        public bool Enqueue(CommandBase commandArg, int priorityArg)
        {
            PriorityCommand priorityCommand = new PriorityCommand(commandArg, priorityArg);
            return Enqueue(priorityCommand);
        }
        public bool Enqueue(CommandBase commandArg, Priority priorityArg)
        {
            return Enqueue(commandArg, (int)priorityArg);
        }

        public bool Enqueue(Action actionArg, int priorityArg)
        {
            return Enqueue(new ActionCommand(actionArg), priorityArg);
        }
        public bool Enqueue(Action actionArg, Priority priorityArg)
        {
            return Enqueue(actionArg, (int)priorityArg);
        }
    }
}
