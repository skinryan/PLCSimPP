namespace CommonLib.TcpSocket
{
    /// <summary>
    /// Each communication message should implement this so the framework
    /// call call ToMessageBytes to get a byte array representation of
    /// the message.
    /// </summary>
    public interface ICommMessage
    {
        /// <summary>
        /// Convert the message to a byte array for transmission.
        /// </summary>
        /// <returns></returns>
        byte[] ToMessageBytes();
    }
}
