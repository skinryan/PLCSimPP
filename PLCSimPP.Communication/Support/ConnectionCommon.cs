using System;
using System.Collections.Generic;
using System.Text;

namespace PLCSimPP.Communication.Support
{
    /// <summary>
    /// Connection States
    /// </summary>
    public enum ConnectionState
    {
        Undefined,
        Open,
        Close,
        Status,
        @Error
    }

    /// <summary>
    /// Transmit Type
    /// </summary>
    public enum TransmitType
    {
        Undefined,
        Send,
        Receive
    }
    
    public class CloseStatus
    {
        #region Constructors
        /// <summary>
        /// CloseStatus Constructor
        /// </summary>
        /// <param name="isSuccessful"></param>
        /// <param name="comment"></param>
        public CloseStatus(bool isSuccessful, string comment)
        {
            this.IsSuccessful = isSuccessful;
            this.Comment = "None.";

            if (!string.IsNullOrEmpty(comment))
                this.Comment = Comment;
        }
        #endregion

        #region Properties
        public bool IsSuccessful
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
            var str = new StringBuilder();

            str.Append(IsSuccessful ? "Close Status: Success." : "Close Status: Failed.");
            str.Append("\r\n").Append("   ");
            str.Append("Comment: ").Append(Comment);

            return str.ToString();
        }
        #endregion
    }

    public class OpenStatus
    {
        /// <summary>
        /// OpenStatus Constructor
        /// </summary>
        /// <param name="isSuccessful"></param>
        /// <param name="comment"></param>
        /// <param name="portInfo"></param>
        public OpenStatus(bool isSuccessful, string comment, string portInfo)
        {
            this.IsSuccessful = isSuccessful;
            this.Comment = "None.";
            this.PortInfo = "Undefined.";

            if (!string.IsNullOrEmpty(comment))
                this.Comment = comment;

            if (!string.IsNullOrEmpty(portInfo))
                this.PortInfo = portInfo;
        }

        #region Properites
        public bool IsSuccessful
        {
            get;
            private set;
        }

        public string Comment
        {
            get;
            private set;
        }

        public string PortInfo
        {
            get;
            private set;
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            var str = new StringBuilder();
            str.Append(IsSuccessful ? "Open Status: Success." : "Open Status: Failed.");
            str.Append("\r\n").Append("   ");
            str.Append("Comment: ").Append(Comment);
            str.Append("\r\n").Append("   ");
            str.Append("PortInfo: ").Append(PortInfo);

            return str.ToString();
        }

        #endregion
    }


}
