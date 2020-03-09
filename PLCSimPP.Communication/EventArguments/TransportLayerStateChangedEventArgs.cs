using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using PLCSimPP.Communication.Support;

namespace PLCSimPP.Communication.EventArguments
{
    public class TransportLayerStateChangedEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// TransportStateChangedEventArgs Constructor
        /// </summary>
        /// <param name="state"></param>
        /// <param name="comment"></param>
        public TransportLayerStateChangedEventArgs(ConnectionState state, string comment)
        {
            this.Comment = "None.";
            this.State = state;

            if (!string.IsNullOrEmpty(comment))
            {
                Comment = comment;
            }
        }

        #endregion

        #region Declarations
        public ConnectionState State
        {
            get;
            private set;
        }

        public string Comment
        {
            get;
            private set;
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            var str = string.Format(CultureInfo.InvariantCulture, "State: {0}.  Comment: {1}", State.ToString("g"), Comment);
            return str;
        }
        #endregion
    }
}
