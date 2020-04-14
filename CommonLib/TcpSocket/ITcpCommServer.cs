using System;

namespace CommonLib.TcpSocket
{
    /// <summary>
    /// The interface for a comm server.
    /// </summary>
    interface ITcpCommServer : ITcpComm
    {
        /// <summary>
        /// Listen for a client connection of the specified port.  When a client
        /// connects, the onClientConnectedCallbackArg is invoked.
        /// </summary>
        /// <param name="serverPortNumberArg"></param>
        /// <param name="onClientConnectedCallbackArg"></param>
        void ListenForClient(int serverPortNumberArg, Action onClientConnectedCallbackArg);
    }
}
