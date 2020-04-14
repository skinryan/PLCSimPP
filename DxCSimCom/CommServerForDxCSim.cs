using System;
using CommonLib.TcpSocket;

namespace DxCSimCom
{
    /// <summary>
    /// This is the server that DXCSim will connect to
    /// </summary>
    [Serializable]
    public class CommServerForDxCSim
    {
        private static CommServerForDxCSim sInstance;

        /// <summary>
        /// Instance of communication server for DCSim
        /// </summary>
        public static CommServerForDxCSim Instance
        {
            get
            {
                if (sInstance == null)
                {
                    sInstance = new CommServerForDxCSim();
                }

                return sInstance;
            }
        }

        /// <summary>
        /// The TCP/IP port number used to communicate with DXCSim.
        /// This port number is dynamicly generated before connection
        /// </summary>
        public int ServerPortNumber { get; private set; }

        /// <summary>
        /// To check whetherthe DXCSim got connected
        /// </summary>
        public bool HasConnectedClient { get; set; }

        private readonly TcpCommServer mTcpCommServer = new TcpCommServer();

        /// <summary>
        /// Constructor
        /// </summary>
        public CommServerForDxCSim()
        {
            HasConnectedClient = false;
            //Instance = this;
        }

        /// <summary>
        /// Start TCP/IP Server
        /// </summary>
        public void StartUp()
        {
            var freePortFinder = new FreePortFinder();
            ServerPortNumber = freePortFinder.GetAnAvailablePort();
            mTcpCommServer.ListenForClient(ServerPortNumber, ClientConnectedCallback);
        }

        /// <summary>
        /// Send message to DxCSim
        /// </summary>
        /// <param name="commMessage"></param>
        public void Send(ICommMessage commMessage)
        {
            mTcpCommServer.Send(commMessage.ToMessageBytes());
        }

        /// <summary>
        /// Disconnect the Server
        /// </summary>
        public void ShutDown()
        {
            mTcpCommServer.Disconnect();
        }

        /// <summary>
        /// This is the callback that is executed when DcSim makes a connection to LcSim
        /// </summary>
        private void ClientConnectedCallback()
        {
            HasConnectedClient = true;
        }
    }
}
