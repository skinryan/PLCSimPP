using System;
using System.Collections.Generic;
using System.Text;

namespace BCI.PLCSimPP.Comm.Models
{
    /// <summary>
    /// Logging message object for log4net 
    /// </summary>
    public class MsgLog : Prism.Mvvm.BindableBase
    {
        private long mId;
        /// <summary>
        /// ID
        /// </summary>
        public long ID
        {
            get { return mId; }
            set { SetProperty(ref mId, value); }
        }

        private DateTime mTime;

        /// <summary>
        /// log time
        /// </summary>
        public DateTime Time
        {
            get { return mTime; }
            set { SetProperty(ref mTime, value); }
        }

        private string mDirection;

        /// <summary>
        /// To receive or send
        /// </summary>
        public string Direction
        {
            get { return mDirection; }
            set { SetProperty(ref mDirection, value); }
        }

        private string mAddress;

        /// <summary>
        /// unit address
        /// </summary>
        public string Address
        {
            get { return mAddress; }
            set { SetProperty(ref mAddress, value); }
        }

        private string mDetails;

        /// <summary>
        /// Details
        /// </summary>
        public string Details
        {
            get { return mDetails; }
            set { SetProperty(ref mDetails, value); }
        }


        private string mCommand;

        /// <summary>
        /// command
        /// </summary>
        public string Command
        {
            get { return mCommand; }
            set { SetProperty(ref mCommand, value); }
        }
        
        private string mToken;

        /// <summary>
        /// Identifies whether or not in the same communication
        /// </summary>
        public string Token
        {
            get { return mToken; }
            set { SetProperty(ref mToken, value); }
        }
    }
}
