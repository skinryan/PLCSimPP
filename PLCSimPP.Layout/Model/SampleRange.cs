using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCI.PLCSimPP.Comm;
using Prism.Mvvm;

namespace BCI.PLCSimPP.Layout.Model
{
    public class SampleRange : BindableBase
    {
        private string mCharacters;

        public string Characters
        {
            get { return mCharacters; }
            set { SetProperty(ref mCharacters, value); }
        }

        private int mAppend;

        public int Append
        {
            get { return mAppend; }
            set { SetProperty(ref mAppend, value); }
        }

        private int mStartNum;

        public int StartNum
        {
            get { return mStartNum; }
            set { SetProperty(ref mStartNum, value); }
        }

        private int mStopNum;

        public int StopNum
        {
            get { return mStopNum; }
            set
            {
                SetProperty(ref mStopNum, value);
                if (mStopNum > 0)
                {
                    QuantityEnable = false;
                }
                else
                {
                    QuantityEnable = true;
                }
            }
        }

        private int mQuantity;

        public int Quantity
        {
            get { return mQuantity; }
            set
            {
                SetProperty(ref mQuantity, value);
                if (mQuantity > 0)
                {
                    EndNumEnable = false;
                }
                else
                {
                    EndNumEnable = true;
                }
            }
        }


        private int mTubeWidth;

        public int TubeWidth
        {
            get { return mTubeWidth; }
            set { SetProperty(ref mTubeWidth, value); }
        }

        private int mTubeHeight;

        public int TubeHeight
        {
            get { return mTubeHeight; }
            set { SetProperty(ref mTubeHeight, value); }
        }

        private RackType mRackType;

        public RackType RackType
        {
            get { return mRackType; }
            set { SetProperty(ref mRackType, value); }
        }


        private bool mQuantityEnable;

        public bool QuantityEnable
        {
            get { return mQuantityEnable; }
            set { SetProperty(ref mQuantityEnable, value); }
        }

        private bool mEndNumEnable;

        public bool EndNumEnable
        {
            get { return mEndNumEnable; }
            set { SetProperty(ref mEndNumEnable, value); }
        }


        private string mDcToken;

        public string DcToken
        {
            get { return mDcToken; }
            set { SetProperty(ref mDcToken, value); }
        }

        private string mDxCToken;

        public string DxCToken
        {
            get { return mDxCToken; }
            set { SetProperty(ref mDxCToken, value); }
        }


        public SampleRange()
        {
            EndNumEnable = true;
            QuantityEnable = true;
            Append = 4;
        }
    }


}
