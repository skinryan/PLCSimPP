using System;

namespace CommonLib.CommandDispatching.Command
{
    public abstract class ComparableCommand : CommandBase, IComparable
    {
        private static long sRequestOrderCounter;
        private readonly long mRequestOrder = sRequestOrderCounter++; //the order the request for execution was made

        private CommandBase WrappedCommand { get; set; }

        protected ComparableCommand(CommandBase commandArg)
        {
            WrappedCommand = commandArg;
        }

        #region ICommand Members

        public override void Execute()
        {
            WrappedCommand.Execute();
        }

        #endregion

        #region IComparable Members

        public virtual int CompareTo(object obj)
        {
            int result = (int)(mRequestOrder - ((ComparableCommand)obj).mRequestOrder);
            return result;
        }

        #endregion
    }
}
