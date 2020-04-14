using System;

namespace CommonLib.TcpSocket
{
    /// <summary>
    /// Implementations of this interface is provided by the client
    /// to the framework.  The framework will use CreateMessageFromBytes
    /// to try to create a new message from the raw message bytes.  
    /// If successful, the framework will then call OnMessageReceivedCallback 
    /// with the new message for the client to process.
    /// </summary>
    public interface IToCommMessageConverterWithCallback
    {
        /// <summary>
        /// Convert the message bytes to a message.
        /// If unsuccessful, null is returned.
        /// If successful, consult MessageWithByteSize.MessageByteLength to find how
        ///   many bytes from messageBytesArg were used to construct the message.
        ///   This is used to strip the used bytes from messageBytesArg so the
        ///   next message can be formed/generated.
        /// </summary>
        /// <param name="messageBytesArg"></param>
        /// <returns></returns>
        MessageWithByteSize CreateMessageFromBytes(byte[] messageBytesArg);

        /// <summary>
        /// If a message can be formed from the raw message bytes, this callback will 
        /// be invoked with the message.
        /// </summary>
        Action<ICommMessage> OnMessageReceivedCallback
        {
            get;
        }
    }
}
