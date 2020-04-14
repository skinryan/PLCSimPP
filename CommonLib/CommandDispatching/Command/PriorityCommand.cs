using System;

namespace CommonLib.CommandDispatching.Command
{
    internal class PriorityCommand : ComparableCommand
    {
        private readonly int mUserPriority;

        internal PriorityCommand(CommandBase commandArg, int priorityArg)
            : base(commandArg)
        {
            mUserPriority = priorityArg;
        }

        #region IComparable Members

        public override int CompareTo(object obj)
        {
            PriorityCommand rightHandSide = obj as PriorityCommand;
            if( rightHandSide == null )
            {
                throw new ArgumentException("Parameter must be of type PriorityCommand!");
            }
            if( mUserPriority != rightHandSide.mUserPriority )
            {
                return mUserPriority - rightHandSide.mUserPriority;
            }
            return base.CompareTo(obj);
        }

        #endregion
    }
}
