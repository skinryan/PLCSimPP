using Prism.Mvvm;
using System;

namespace BCI.PLCSimPP.Service.Log
{
    public class LogGroup : BindableBase
    {
        private string mToken;
        /// <summary>
        /// Token
        /// </summary>
        public string Token
        {
            get { return mToken; }
            set
            {
                SetProperty(ref mToken, value);
            }
        }

        private int mMsgCount;
        /// <summary>
        /// MsgCount
        /// </summary>
        public int MsgCount
        {
            get { return mMsgCount; }
            set
            {
                SetProperty(ref mMsgCount, value);
            }
        }

        private DateTime mStartTime;
        /// <summary>
        /// StartTime
        /// </summary>
        public DateTime StartTime
        {
            get { return mStartTime; }
            set
            {
                SetProperty(ref mStartTime, value);
            }
        }

    }
}
