using System;
using System.Collections.Generic;
using System.Text;

namespace PLCSimPP.Communication.EventArguments
{
    public class TransportLayerDebugDataEventArgs : EventArgs
    {
        #region Enum
        public enum TransmitType
        {
            Undefined,
            Send,
            Receive
        }
        #endregion

        #region TransmitType
        public TransmitType Type
        {
            get;
            private set;
        }

        public string TransmitData
        {
            get;
            private set;
        }
        #endregion

        #region Constructor
        public TransportLayerDebugDataEventArgs(TransmitType type, string transmitData)
        {
            this.Type = type;
            this.TransmitData = transmitData;
        }
        #endregion
    }
}
