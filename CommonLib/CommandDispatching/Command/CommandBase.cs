namespace CommonLib.CommandDispatching.Command
{
    public abstract class CommandBase
    {
        public MessageBase Message { get; protected set; }

        internal string Description { get; set; }

        public override string ToString()
        {
            if (Description != null)
            {
                return Description;
            }

            return base.ToString();
        }

        public abstract void Execute();
    }
}
