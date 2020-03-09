using System;
using System.Collections.Generic;
using System.Text;
using PLCSimPP.Communication.Models;

namespace PLCSimPP.Communication.EventArguments
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
        public TransportLayerDataReceivedEventArgs(CmdMsg receivedString)
        {
            ReceivedString = receivedString;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets or Sets the received string
        /// </summary>
        public CmdMsg ReceivedString
        {
            get;
            private set;
        }
        #endregion
    }
}
