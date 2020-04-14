namespace PLCSimPP.Comm.Interfaces
{
    public interface IMessage
    {
        /// <summary>
        /// Message target address
        /// </summary>
        string UnitAddr { get; set; }

        /// <summary>
        /// Message command
        /// </summary>
        string Command { get; set; }

        /// <summary>
        /// Message content
        /// </summary>
        string Param { get; set; }

        /// <summary>
        /// The device belongs to the port
        /// </summary>
        int Port { get; set; }

        /// <summary>
        /// Build messagelog
        /// </summary>
        /// <returns></returns>
        string ToString();
    }
}