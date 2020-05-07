using System;
using System.Collections.Generic;
using System.Text;
using PLCSimPP.Comm.Interfaces;
using Prism.Mvvm;

namespace PLCSimPP.Comm.Models
{
    public class Sample : BindableBase, ISample
    {
        private string _sampleID;

        /// <inheritdoc/>
        public string SampleID
        {
            get { return _sampleID; }
            set { SetProperty(ref _sampleID, value); }
        }

        private RackType mRack;
        public RackType Rack
        {
            get { return mRack; }
            set { SetProperty(ref mRack, value); }
        }


        //private int mWidth;
        //public int Width
        //{
        //    get { return mWidth; }
        //    set { SetProperty(ref mWidth, value); }
        //}


        //private int mHeight;
        //public int Height
        //{
        //    get { return mHeight; }
        //    set { SetProperty(ref mHeight, value); }
        //}

        private bool mIsLoaded;

        public bool IsLoaded
        {
            get { return mIsLoaded; }
            set { SetProperty(ref mIsLoaded, value); }
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

        private bool mRetrieving;

        public bool Retrieving
        {
            get { return mRetrieving; }
            set { SetProperty(ref mRetrieving, value); }
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
