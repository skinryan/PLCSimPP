using System;
using System.Collections.Generic;
using System.Text;
using BCI.PLCSimPP.Comm.Enums;
using BCI.PLCSimPP.Comm.Interfaces;
using Prism.Mvvm;

namespace BCI.PLCSimPP.Comm.Models
{
    /// <summary>
    /// The implementation of ISample
    /// </summary>
    public class Sample : BindableBase, ISample
    {
        private string mSampleID;

        /// <inheritdoc/>
        public string SampleID
        {
            get { return mSampleID; }
            set { SetProperty(ref mSampleID, value); }
        }

        private RackType mRack;
        /// <inheritdoc/>
        public RackType Rack
        {
            get { return mRack; }
            set { SetProperty(ref mRack, value); }
        }

        private bool mIsLoaded;
        /// <inheritdoc/>
        public bool IsLoaded
        {
            get { return mIsLoaded; }
            set { SetProperty(ref mIsLoaded, value); }
        }

        private string mDcToken;
        /// <inheritdoc/>
        public string DcToken
        {
            get { return mDcToken; }
            set { SetProperty(ref mDcToken, value); }
        }

        private string mDxCToken;
        /// <inheritdoc/>
        public string DxCToken
        {
            get { return mDxCToken; }
            set { SetProperty(ref mDxCToken, value); }
        }

        private bool mRetrieving;
        /// <inheritdoc/>
        public bool Retrieving
        {
            get { return mRetrieving; }
            set { SetProperty(ref mRetrieving, value); }
        }

        private bool mIsSubTube;
        /// <inheritdoc/>
        public bool IsSubTube
        {
            get { return mIsSubTube; }
            set { SetProperty(ref mIsSubTube, value); }
        }


    }
}
