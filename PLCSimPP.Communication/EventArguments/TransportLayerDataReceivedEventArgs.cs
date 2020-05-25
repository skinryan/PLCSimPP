using System;
using System.Collections.Generic;
using System.Text;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Communication.Models;

namespace BCI.PLCSimPP.Communication.EventArguments
{
    /// <summary>
    /// The event arguments used in PLCSim communication transport
    /// layer when data is received.
    /// </summary>
    public class TransportLayerDataReceivedEventArgs : EventArgs
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public TransportLayerDataReceivedEventArgs(IMessage receivedString)
        {
            ReceivedMsg = receivedString;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets or Sets the received string
        /// </summary>
        public IMessage ReceivedMsg
        {
            get;
            private set;
        }
        #endregion
    }
}
