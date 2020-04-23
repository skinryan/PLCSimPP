using Prism.Mvvm;
using System;

namespace PLCSimPP.Service.Log
{
    public class LogGroup : BindableBase
    {
        private string mToken;

        public string Token
        {
            get { return mToken; }
            set
            {
                SetProperty(ref mToken, value);
            }
        }

        private int mMsgCount;

        public int MsgCount
        {
            get { return mMsgCount; }
            set
            {
                SetProperty(ref mMsgCount, value);
            }
        }

        private DateTime mStartTime;

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
