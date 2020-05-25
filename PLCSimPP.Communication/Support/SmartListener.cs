using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Models;
using BCI.PLCSimPP.Communication.EventArguments;
using BCI.PLCSimPP.Communication.Interface;
using BCI.PLCSimPP.Communication.Models;

namespace BCI.PLCSimPP.Communication.Support
{
    public class SmartListener
    {


        #region "Constructor"

        public SmartListener()
        {
        }

        #endregion // Constructor

        #region "Variables/Events"

        private int m_Port;
        private TcpListener m_Listener;
        private Thread m_ListenThread;
        private IServer m_IServer;
        public delegate void DataReceivedEventHandler(object sender, TransportLayerDataReceivedEventArgs e);  //Added correct delegate arguments// RT
        private DataReceivedEventHandler DataReceivedEvent;

        public event DataReceivedEventHandler DataReceived
        {
            add
            {
                DataReceivedEvent = (DataReceivedEventHandler)System.Delegate.Combine(DataReceivedEvent, value);
            }
            remove
            {
                DataReceivedEvent = (DataReceivedEventHandler)System.Delegate.Remove(DataReceivedEvent, value);
            }
        }

        public delegate void StateChangedEventHandler(object sender, TransportLayerStateChangedEventArgs e);  //Added correct delegate arguments// RT
        private StateChangedEventHandler StateChangedEvent;

        public event StateChangedEventHandler StateChanged
        {
            add
            {
                StateChangedEvent = (StateChangedEventHandler)System.Delegate.Combine(StateChangedEvent, value);
            }
            remove
            {
                StateChangedEvent = (StateChangedEventHandler)System.Delegate.Remove(StateChangedEvent, value);
            }
        }


        #endregion // Variables/Events

        #region "Listen"

        //<SocketPermissionAttribute(SecurityAction.Demand, Unrestricted:=True)> _
        public void ListenOn(string serverAddress, int port)
        {
            if (m_ListenThread != null)
            {
                // Called a second time - reset.
                this.Close();
            }

            m_Port = port;
            IPAddress localAddr = IPAddress.Parse(serverAddress);

            if (localAddr == null)
            {
                throw (new SmartListenerException(string.Format(CultureInfo.InvariantCulture, "localAddr is Nothing", null)));
            }
            else
            {
                m_Listener = new TcpListener(localAddr, m_Port);

                m_ListenThread = new Thread(new System.Threading.ThreadStart(DoListen));
                m_ListenThread.Name = "SmartListener_Port_" + m_Port;
                m_ListenThread.IsBackground = true;

                m_Listener.Start();
                m_ListenThread.Start();
            }

        }  //Removed extra curly brace// RT

        private void DoListen()
        {
            try
            {
                while (true)
                {
                    //Accepts a pending connection request.  This is a blocking method.
                    TcpClient client = m_Listener.AcceptTcpClient();

                    if (m_IServer == null)
                    {
                        var aServer = new Server();
                        m_IServer = aServer;
                        aServer.SmartConnectionClosedEvent += ConnectionClosedHandler;
                        aServer.SmartConnectionDataReceivedEvent += DataReceivedHandler;
                        m_IServer.ConnectTo(client);
                        if (StateChangedEvent != null)
                            StateChangedEvent(this, new TransportLayerStateChangedEventArgs(ConnectionState.Open, "Open"));
                    }
                    else
                    {
                        client.Close();
                    }

                }
            }
            catch (ThreadAbortException)
            {
                // Do nothing.  Thread is aborting.
            }
            catch (SocketException ex)
            {
                // Exception occurs if another server is already using address and port.
                Console.Write(ex.ToString());
            }
        }

        public void Close()
        {
            CloseServer();

            if (m_ListenThread != null)
            {
                m_ListenThread.Abort();
                m_ListenThread = null;
            }

            if (m_Listener != null)
            {
                m_Listener.Stop();
            }
        }

        private void ConnectionClosedHandler(object sender, EventArgs e)
        {
            CloseServer();
            if (StateChangedEvent != null)
                StateChangedEvent(this, new TransportLayerStateChangedEventArgs(ConnectionState.Close, "Closed"));
        }

        private void CloseServer()
        {
            if (m_IServer != null)
            {
                m_IServer.Close();
                m_IServer = null;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void DataReceivedHandler(object sender, SmartConnectionDataReceivedEventArgs e)
        {
            try
            {
                OnDataReceived(e.Msg);
            }
            catch (System.Exception)
            {
            }
        }

        public void OnDataReceived(Comm.Interfaces.IMessage msgStr)
        {
            // Raise event.
            var evArgs = new TransportLayerDataReceivedEventArgs(msgStr);
            if (DataReceivedEvent != null)
                DataReceivedEvent(this, evArgs);
        }

        #endregion // Listen

        #region "Send"

        public void Send(IMessage msg)
        {
            if (m_IServer != null)
            {
                m_IServer.SendToClient(msg);
            }
        }

        #endregion // Send

    }


    [Serializable()]
    public class SmartListenerException : System.Exception
    {


        public SmartListenerException()
        {
        }

        public SmartListenerException(string message)
            : base(message)
        {
        }

        public SmartListenerException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected SmartListenerException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
