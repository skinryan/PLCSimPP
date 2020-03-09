using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml.Serialization;
using PLCSimPP.Comm.Interfaces;
using Prism.Mvvm;

namespace PLCSimPP.Service.Devicies
{
    [XmlInclude(typeof(Aliquoter))]
    public abstract class UnitBase : BindableBase
    {
        private string mDisplayName;

        public string DisplayName
        {
            get { return mDisplayName; }
            set { mDisplayName = value; }
        }

        private int mPort;

        public int Port
        {
            get { return mPort; }
            set { SetProperty(ref mPort, value); }
        }

        private int mPendingCount;
        public int PendingCount
        {
            get { return mPendingCount; }
            set { this.SetProperty(ref mPendingCount, value); }
        }

        private string mAddress;
        public string Address
        {
            get { return mAddress; }
            set { this.SetProperty(ref mAddress, value); }
        }

        private UnitType mUnitType;
        public UnitType UnitType
        {
            get { return mUnitType; }
            set { this.SetProperty(ref mUnitType, value); }
        }

        private ObservableCollection<UnitBase> mChildren;
        public ObservableCollection<UnitBase> Children
        {
            get { return mChildren; }
            //set { this.SetProperty(ref _children, value); }
        }

        private bool mHasChild;
        public bool HasChild
        {
            get { return mHasChild; }
            set { this.SetProperty(ref mHasChild, value); }
        }

        private UnitBase mParent;

        [XmlIgnore]
        public UnitBase Parent
        {
            get { return mParent; }
            set { this.SetProperty(ref mParent, value); }
        }

        private Sample mCurrentSample;
        public Sample CurrentSample
        {
            get { return mCurrentSample; }
            set { this.SetProperty(ref mCurrentSample, value); }
        }


        public abstract void EnqueueSample(Sample sample);

        public abstract void MoveSample(SortingOrder order, string bcrNo, Direction direction = Direction.Forward);

        public abstract void OnReceivedMsg(string cmd, string content);

        public abstract void ResetQueue();

        public abstract bool TryDequeueSample(out Sample sample);

        public UnitBase()
        {
            mChildren = new ObservableCollection<UnitBase>();
        }

        public UnitBase(int port, string address, string display)
        {
            mPort = port;
            mAddress = address;
            mDisplayName = display;
            mChildren = new ObservableCollection<UnitBase>();
        }
    }
}
