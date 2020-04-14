using System;

namespace CommonLib.CommandDispatching.Command
{
    internal class ActionCommand : CommandBase
    {
        private readonly Action mAction;

        public ActionCommand(Action actionArg)
        {
            mAction = actionArg;
        }

        public override void Execute()
        {
            mAction();
        }
    }
}
