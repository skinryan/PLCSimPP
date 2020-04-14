using System;
using CommonLib.CommandDispatching.Command;

namespace CommonLib.CommandDispatching.Dispatcher
{
    /// <summary>
    /// CommandDispatcher that executes commands in a FIFO order.
    /// </summary>
    public class QueueCommandDispatcher : CommandDispatcherBase<CommandBase>
    {
        public QueueCommandDispatcher(string descriptionArg)
            : base(descriptionArg)
        {
        }

        public new bool Enqueue(CommandBase commandArg)
        {
            return base.Enqueue(commandArg);
        }

        public bool Enqueue(Action actionArg)
        {
            return Enqueue(new ActionCommand(actionArg));
        }

        protected override void Insert(CommandBase commandArg)
        {
            pCommandList.Add(commandArg);
        }
    }
}
