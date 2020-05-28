using System;
using CommonLib.TcpSocket;

namespace DcSimCom
{
    /// <summary>
    /// This is the server that DcSim will connect to.
    /// </summary>
    [Serializable]
    public class CommServerForDcSim : IDisposable
    {
        private static CommServerForDcSim sInstance;

        /// <summary>
        /// Instance of communication server for DCSim
        /// </summary>
        public static CommServerForDcSim Instance
        {
            get
            {
                if (sInstance == null)
                {
                    sInstance = new CommServerForDcSim();
                }

                return sInstance;
            }
        }

        /// <summary>
        /// TCP/IP communication port with DCSim
        /// This port is dynamicly generated
        /// </summary>
        public int ServerPortNumber { get; private set; }

        /// <summary>
        /// To check whether the DCSim has connected or not
        /// </summary>
        public bool HasConnectedClient { get; set; }

        private TcpCommServer mTcpCommServer;

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            //mTcpCommServer.OnDisconnectedCallback
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private CommServerForDcSim()
        {
            HasConnectedClient = false;

            //Instance = this;
        }

        /// <summary>
        /// Startup Server
        /// </summary>
        public void StartUp()
        {
            mTcpCommServer = new TcpCommServer();
            var freePortFinder = new FreePortFinder();
            ServerPortNumber = freePortFinder.GetAnAvailablePort();
            mTcpCommServer.ListenForClient(ServerPortNumber, ClientConnectedCallback);
        }

        /// <summary>
        /// Send message to DCSim
        /// </summary>
        /// <param name="commMessageArg"></param>
        public void Send(ICommMessage commMessageArg)
        {
            mTcpCommServer.Send(commMessageArg.ToMessageBytes());
        }

        /// <summary>
        /// Disconnect Server
        /// </summary>
        public void ShutDown()
        {
            mTcpCommServer.Disconnect();
        }

        /// <summary>
        /// This is the callback that is executed when DcSim makes a connection to LcSim.
        /// </summary>
        private void ClientConnectedCallback()
        {
            HasConnectedClient = true;
        }
    }
}