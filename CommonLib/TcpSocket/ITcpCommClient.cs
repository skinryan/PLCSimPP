namespace CommonLib.TcpSocket
{
    /// <summary>
    /// The interface for a comm client.
    /// </summary>
    interface ITcpCommClient : ITcpComm
    {
        /// <summary>
        /// Connect to a server.
        /// </summary>
        /// <param name="serverIpAddressTextArg">Server IP address (e.g., 127.0.0.1) or domain name.</param>
        /// <param name="serverPortNumberArg">Server port number.</param>
        void ConnectToServer (string serverIpAddressTextArg, int serverPortNumberArg);
    }
}
