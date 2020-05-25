using System;
using System.Collections.Generic;
using System.Text;
using BCI.PLCSimPP.Communication.EventArguments;
using BCI.PLCSimPP.Communication.Support;

namespace BCI.PLCSimPP.Communication.Interface
{
    public delegate void TransportLayerDataReceivedEventHandler(object sender, TransportLayerDataReceivedEventArgs e);
    public delegate void TransportLayerStateChangedEventHandler(object sender, TransportLayerStateChangedEventArgs e);
    public delegate void TransportLayerDebugDataEventHandler(object sender, TransportLayerDebugDataEventArgs e);

    public interface IConnection : IDisposable
    {
        /// <summary>
        /// Get connection name
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// Get connection count number
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// Get connection state
        /// </summary>
        ConnectionState State
        {
            get;
        }

        event TransportLayerDataReceivedEventHandler TransportLayerDataReceivedEvent;
        event TransportLayerDebugDataEventHandler TransportLayerDebugDataEvent;
        event TransportLayerStateChangedEventHandler TransportLayerStateChangedEvent;

        #region "Methods"
        void Write(string transmitString);
        OpenStatus Open();
        CloseStatus Close();
        #endregion
    }
}
