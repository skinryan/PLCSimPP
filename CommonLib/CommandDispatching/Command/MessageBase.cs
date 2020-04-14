namespace CommonLib.CommandDispatching.Command
{
    public abstract class MessageBase
    {
        public string Name { get; set; }

        protected MessageBase(string nameArg)
        {
            Name = nameArg;
        }

        public abstract string HeaderDump();
    }
}
