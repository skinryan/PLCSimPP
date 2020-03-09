namespace PLCSimPP.Comm.Interfaces
{
    public interface IMessage
    {
        /// <summary>
        /// Message target address
        /// </summary>
        string Address { get; set; }

        /// <summary>
        /// Message command
        /// </summary>
        string Command { get; set; }

        /// <summary>
        /// Message content
        /// </summary>
        string Content { get; set; }

        /// <summary>
        /// Build messagelog
        /// </summary>
        /// <returns></returns>
        string ToString();
    }
}