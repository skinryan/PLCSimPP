using System;

namespace CommonLib.TcpSocket
{
    /// <summary>
    /// The base interface for TcpCommClient and TcpCommServer.
    /// </summary>
    interface ITcpComm : IDisposable
    {
        /// <summary>
        /// For each message, need to tell the framework how to convert the raw message bytes
        /// into the message and how to process a completed message.
        /// </summary>
        /// <param name="toCommMessageConverterWithCallback"></param>
        void AddCommMessageConverterWithCallback(IToCommMessageConverterWithCallback toCommMessageConverterWithCallback);

        /// <summary>
        /// Callback used when the connection ends.
        /// </summary>
        Action OnDisconnectedCallback { get; set; }

        /// <summary>
        /// Disconnect the connection.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Send the specified bytes through the connection.
        /// </summary>
        /// <param name="bytesArg"></param>
        void Send(byte[] bytesArg);
    }
}
