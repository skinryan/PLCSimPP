using System;
using Prism.Mvvm;

namespace PLCSimPP.Service.Log
{
    public class LogContent : BindableBase
    {


        private DateTime mTime;

        /// <summary>
        /// log time
        /// </summary>
        public DateTime Time
        {
            get
            {
                return mTime;
            }
            set
            {
                SetProperty(ref mTime, value);
            }
        }

        private string mDirection;

        /// <summary>
        /// To receive or send
        /// </summary>
        public string Direction
        {
            get
            {
                return mDirection;
            }
            set
            {
                SetProperty(ref mDirection, value);
            }
        }

        private string mAddress;

        /// <summary>
        /// unit address
        /// </summary>
        public string Address
        {
            get
            {
                return mAddress;
            }
            set
            {
                SetProperty(ref mAddress, value);
            }
        }

        private string mDetails;

        /// <summary>
        /// Details
        /// </summary>
        public string Details
        {
            get
            {
                return mDetails;
            }
            set
            {
                SetProperty(ref mDetails, value);
            }
        }


        private string mCommand;

        /// <summary>
        /// command
        /// </summary>
        public string Command
        {
            get
            {
                return mCommand;
            }
            set
            {
                SetProperty(ref mCommand, value);
            }
        }



        private string mToken;

        /// <summary>
        /// Identifies whether or not in the same communication
        /// </summary>
        public string Token
        {
            get
            {
                return mToken;
            }
            set
            {
                SetProperty(ref mToken, value);
            }
        }

        public LogContent(string direction, string address, string command, string para, string token, int status = 0)
        {
            this.Time = DateTime.Now;
            this.Address = address;
            this.Command = command;
            this.Details = para;
            this.Direction = direction;
            this.Token = token;
        }

        public LogContent()
        {

        }
    }
}
