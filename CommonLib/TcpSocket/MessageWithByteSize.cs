namespace CommonLib.TcpSocket
{
    /// <summary>
    /// Contains the message and the number of raw bytes that were used
    /// to create the message.
    /// </summary>
    public class MessageWithByteSize
    {
        public ICommMessage Message { get; private set; }
        public int MessageByteLength { get; private set; }

        public MessageWithByteSize(ICommMessage messageArg, int messageByteSizeArg)
        {
            Message = messageArg;
            MessageByteLength = messageByteSizeArg;
        }
    }
}