using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Models;
using BCI.PLCSimPP.Communication.EventArguments;
using BCI.PLCSimPP.Communication.Interface;
using BCI.PLCSimPP.Communication.Models;
using BCI.PLCSimPP.Communication.Support;

namespace BCI.PLCSimPP.Communication
{
    public class TcpIpServerConnection : ITcpIpServerConnection
    {
        #region "Constructors"

        public TcpIpServerConnection()
        {
            mName = "Undefined";

            //SafeIncrement.Increment(ref mCount);
            this.OnUpdateState(ConnectionState.Undefined, null);
        }

        #endregion

        #region "Variables/Properties"

        private SmartListener mSmartListener;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SmartListener SmartListener
        {
            [DebuggerHidden()]
            get
            {
                return mSmartListener;
            }
        }

        private string mAddress;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private string Address
        {
            [DebuggerHidden()]
            get
            {
                return mAddress;
            }
        }

        private int mPort;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private int Port
        {
            [DebuggerHidden()]
            get
            {
                return mPort;
            }
        }

        string mName;
        public string Name
        {
            [DebuggerHidden()]
            get
            {
                return mName;
            }
        }

        readonly int mCount;
        public int Count
        {
            [DebuggerHidden()]
            get
            {
                return mCount;
            }
        }

        ConnectionState mState;
        public ConnectionState State
        {
            [DebuggerHidden()]
            get
            {
                return mState;
            }
        }

        #endregion

        #region "Events"
        //private delegate void DataReceivedEventHandler(object sender, TransportLayerDataReceivedEventArgs e); //Manually removed// RT
        private TransportLayerDataReceivedEventHandler mTransportLayerDataReceived;
        public event TransportLayerDataReceivedEventHandler TransportLayerDataReceivedEvent
        {
            add
            {
                mTransportLayerDataReceived = (TransportLayerDataReceivedEventHandler)Delegate.Combine(mTransportLayerDataReceived, value);
            }
            remove
            {
                mTransportLayerDataReceived = (TransportLayerDataReceivedEventHandler)Delegate.Remove(mTransportLayerDataReceived, value);
            }
        }

        //private delegate void TransportLayerDebugDataEventHandler(object sender, TransportLayerDebugDataEventArgs e); //Manually removed// RT
        private TransportLayerDebugDataEventHandler mTransportLayerDebugData;

        public event TransportLayerDebugDataEventHandler TransportLayerDebugDataEvent
        {
            add
            {
                mTransportLayerDebugData = (TransportLayerDebugDataEventHandler)Delegate.Combine(mTransportLayerDebugData, value);
            }
            remove
            {
                mTransportLayerDebugData = (TransportLayerDebugDataEventHandler)Delegate.Remove(mTransportLayerDebugData, value);
            }
        }

        //private delegate void TransportLayerStateChangedEventHandler(object sender, TransportLayerStateChangedEventArgs e); //Manually removed// RT
        private TransportLayerStateChangedEventHandler mTransportLayerStateChanged;

        public event TransportLayerStateChangedEventHandler TransportLayerStateChangedEvent
        {
            add
            {
                mTransportLayerStateChanged = (TransportLayerStateChangedEventHandler)Delegate.Combine(mTransportLayerStateChanged, value);
            }
            remove
            {
                mTransportLayerStateChanged = (TransportLayerStateChangedEventHandler)Delegate.Remove(mTransportLayerStateChanged, value);
            }
        }


        #endregion

        #region "Methods"

        #region "Open/Close"

        public OpenStatus Open()
        {
            return this.OpenInvalid();
        }

        public OpenStatus OpenInvalid()
        {
            var status = new OpenStatus(false, "Invalid Open method.  Use ITcpIpServerLisConnection.Open instead.", string.Empty);
            return status;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public OpenStatus Open(string name, string address, int port)
        {
            bool errFound = false;
            string errStr = string.Empty;

            try
            {
                mName = name;
                mAddress = address;
                mPort = port;
                mSmartListener = new SmartListener();
                mSmartListener.DataReceived += new SmartListener.DataReceivedEventHandler(DataReceivedHandler);
                mSmartListener.StateChanged += StateChangedHandler;
                mSmartListener.ListenOn(address, port);
            }
            catch (Exception ex)
            {
                errFound = true;
                errStr = ex.ToString();
            }

            var portInfo = new StringBuilder();
            portInfo.AppendFormat(CultureInfo.InvariantCulture, "Address: {0}.", address);
            portInfo.AppendFormat(CultureInfo.InvariantCulture, "  Port: {0}.", port);

            if (!errFound)
            {
                string statusStr = string.Format(CultureInfo.InvariantCulture, "{0} is listening.", mName);
                // Don't call OnUpdateState here, state will be updated once a client connects
                return new OpenStatus(true, statusStr, portInfo.ToString());
            }
            else
            {
                string statusStr = string.Format(CultureInfo.InvariantCulture, "{0}.", errStr);
                this.OnUpdateState(ConnectionState.Error, statusStr);
                return new OpenStatus(false, statusStr, portInfo.ToString());
            }

        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public CloseStatus Close()
        {
            bool errFound = false;
            string errStr = string.Empty;

            try
            {
                if (mSmartListener != null)
                {
                    mSmartListener.Close();
                    mSmartListener = null;
                }
            }
            catch (Exception ex)
            {
                errFound = true;
                errStr = ex.ToString();
            }

            if (!errFound)
            {
                string statusStr = string.Format(CultureInfo.InvariantCulture, "{0} is closed.", mName);
                this.OnUpdateState(ConnectionState.Close, statusStr);
                return new CloseStatus(true, statusStr);
            }
            else
            {
                string statusStr = string.Format(CultureInfo.InvariantCulture, "{0}", errStr);
                this.OnUpdateState(ConnectionState.Error, statusStr);
                return new CloseStatus(false, statusStr);
            }

        }

        #endregion

        #region "Read/Write"

        public void Write(IMessage msg)
        {
            //this.OnDebugData(TransmitType.Send, transmitStr);

            mSmartListener.Send(msg);
        }

        private void DataReceivedHandler(object sender, TransportLayerDataReceivedEventArgs e)
        {
            OnDataReceived(e.ReceivedMsg);
        }

        private void StateChangedHandler(object sender, TransportLayerStateChangedEventArgs e)
        {
            OnUpdateState(e.State, e.Comment);
        }

        public void OnDataReceived(Comm.Interfaces.IMessage msgStr)
        {
            // this.OnDebugData(TransmitType.Receive, msgStr);

            // Raise event.
            var evArgs = new TransportLayerDataReceivedEventArgs(msgStr);
            if (mTransportLayerDataReceived != null)
                mTransportLayerDataReceived(this, evArgs);
        }

        #endregion

        #region "RaiseEventHelpers"

        //private void OnDebugData(TransmitType type, CmdMsg data)
        //{
        //    TransportLayerDebugDataEventArgs.TransmitType debugType;

        //    switch (type)
        //    {
        //        case TransmitType.Receive:

        //            debugType = TransportLayerDebugDataEventArgs.TransmitType.Receive;
        //            break;
        //        case TransmitType.Send:

        //            debugType = TransportLayerDebugDataEventArgs.TransmitType.Send;
        //            break;
        //        default:

        //            debugType = TransportLayerDebugDataEventArgs.TransmitType.Undefined;
        //            break;
        //    }

        //    string str = string.Format(CultureInfo.CurrentCulture, "{0}: {1}", StringFunctions.CurrentTime, HostHelper.CString(data));

        //    var evArgs = new TransportLayerDebugDataEventArgs(debugType, str);
        //    if (mTransportLayerDebugData != null)
        //        mTransportLayerDebugData(this, evArgs);
        //}

        private void OnUpdateState(ConnectionState state, string comment)
        {
            // Update state.
            mState = state;

            // Raise event.
            var evArgs = new TransportLayerStateChangedEventArgs(state, comment);
            if (mTransportLayerStateChanged != null)
                mTransportLayerStateChanged(this, evArgs);
        }

        #endregion

        #endregion

        #region "Dispose"

        private bool mDisposedValue; // To detect redundant calls

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!mDisposedValue)
            {

                // If disposing equals true, dispose all managed and unmanaged resources.
                if (disposing)
                {
                    OnUpdateState(ConnectionState.Status, "Tcp/Ip server comm object was disposed.");
                    // Free managed resources when explicitly called
                }

                // Free shared unmanaged resources.  When an object is executing its finalization code,
                // it should not reference other objects, because finalizers do not execute in any
                // particular order. If an executing finalizer references another object that has
                // already been finalized, the executing finalizer will fail.

            }
            mDisposedValue = true;
        }

        // This Finalize method will run only if the Dispose method does not get called.
        // By default, methods are NotOverridable. This prevents a derived class from overriding this method.
        ~TcpIpServerConnection()
        {
            // Do not re-create Dispose clean-up code here.  Calling Dispose(false) is
            // optimal in terms of readability and maintainability.
            Dispose(false);
        }

        #region " IDisposable Support "

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);

            // Take object of the finalization queue to prevent finalization
            // code for this object from executing a second time.
            GC.SuppressFinalize(this);
        }

        #endregion

        #endregion

    }

}
