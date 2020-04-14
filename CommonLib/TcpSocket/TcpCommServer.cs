using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace CommonLib.TcpSocket
{
    public class TcpCommServer : TcpComm, ITcpCommServer
    {
        private TcpListener mTcpServer;
        private TcpClient mTcpClient;

        // Constructor(s) ==============================================================

        public TcpCommServer(int receiveBufferSizeArg = DEFAULT_UNPROCESSED_RECEIVE_BYTES_BUFFER_SIZE)
            : base(receiveBufferSizeArg)
        {
        }

        // Methods(s) - Public =========================================================

        /// <summary>
        /// Listen for a client connection.
        /// On return, a client may not be connected.  When a client does connect,
        /// OnClientConnectedCallbackArg will be called.
        /// </summary>
        /// <param name="serverPortNumberArg"></param>
        /// <param name="onClientConnectedCallbackArg"></param>
        public void ListenForClient(int serverPortNumberArg, Action onClientConnectedCallbackArg)
        {
            mTcpServer = new TcpListener(IPAddress.Any, serverPortNumberArg);
            mTcpServer.Start();
            Task.Factory.StartNew(() =>
            {
                mTcpClient = mTcpServer.AcceptTcpClient();
                NetStreamComm = mTcpClient.GetStream();
                StartReceivingThread();
                onClientConnectedCallbackArg();
            });
        }

        public override void Disconnect()
        {
            base.Disconnect();
            if (mTcpServer != null)
            {
                mTcpServer.Stop();
                mTcpServer = null;
            }
        }
      
        #region IDisposable

        private bool mDisposed;

        void IDisposable.Dispose()
        {
            Dispose(true);

            // This object will be cleaned up by the Dispose method.
            // Therefore, call GC.SupressFinalize to take this object off the finalization queue
            // and prevent finalization code for this object from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        //   1) If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        //   2) If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be disposed.
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.mDisposed)
            {
                if (disposing)
                {
                    // Dispose managed resources.
                    Disconnect();
                }

                // Clean up unmanaged resources.

                // Note disposing has been done.
                mDisposed = true;
            }
        }

        // Use C# destructor syntax for finalization code.
        // This destructor will run only if the Dispose method does not get called.
        // It gives your base class the opportunity to finalize.
        // Do not provide destructors in types derived from this class.
        ~TcpCommServer()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

        #endregion IDisposable
    }
}