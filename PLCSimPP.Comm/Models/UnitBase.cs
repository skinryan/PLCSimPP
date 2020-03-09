using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using PLCSimPP.Comm.Interfaces;
using Prism.Mvvm;

namespace PLCSimPP.Comm.Models
{

    public abstract class UnitBase : BindableBase, IUnit
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

        private string mAddress;
        public string Address
        {
            get { return mAddress; }
            set { this.SetProperty(ref mAddress, value); }
        }

        private ObservableCollection<IUnit> mChildren;
        public ObservableCollection<IUnit> Children
        {
            get { return mChildren; }
            //set { this.SetProperty(ref _children, value); }
        }

        private int mPendingCount;

        public int PendingCount
        {
            get { return mPendingCount; }
            set { this.SetProperty(ref mPendingCount, value); }
        }

        private bool mHasChild;
        public bool HasChild
        {
            get { return mHasChild; }
            set { this.SetProperty(ref mHasChild, value); }
        }

        private IUnit mParent;

        public IUnit Parent
        {
            get { return mParent; }
            set { this.SetProperty(ref mParent, value); }
        }

        private ISample mCurrentSample;
        public ISample CurrentSample
        {
            get { return mCurrentSample; }
            set { this.SetProperty(ref mCurrentSample, value); }
        }

        public abstract void EnqueueSample(ISample sample);

        public abstract void MoveSample(SortingOrder order, string bcrNo, Direction direction = Direction.Forward);

        public abstract void OnReceivedMsg(string cmd, string content);

        public abstract void ResetQueue();

        public abstract bool TryDequeueSample(out ISample sample);


        public UnitBase()
        {
            mChildren = new ObservableCollection<IUnit>();
        }

        public UnitBase(int port, string address, string display)
        {
            mPort = port;
            mAddress = address;
            mDisplayName = display;
            mChildren = new ObservableCollection<IUnit>();
        }



    }
}
