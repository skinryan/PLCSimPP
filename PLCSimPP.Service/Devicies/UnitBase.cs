using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using CommonServiceLocator;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Comm.Interfaces.Services;
using PLCSimPP.Service.Devicies.StandardResponds;
using PLCSimPP.Comm.Constants;
using PLCSimPP.Comm;
using Prism.Mvvm;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading;
using PLCSimPP.Comm.Models;

namespace PLCSimPP.Service.Devicies
{

    public abstract class UnitBase : BindableBase, IUnit
    {
        protected readonly ISendMsgBehavior mSendBehavior;
        protected readonly ILogService mLogger;
        protected readonly IRouterService mRouterService;
        protected ConcurrentQueue<ISample> mPendingQueue;
        protected Task mWaitArrivalTask;
        protected int mArrivalInterval = 1000;

        private string mDisplayName;

        public string DisplayName
        {
            get { return mDisplayName; }
            set { SetProperty(ref mDisplayName, value); }
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
        }


        public int PendingCount
        {
            get { return mPendingQueue.Count; }
        }

        public bool HasChild
        {
            get { return Children.Count > 0; }
        }

        private IUnit mParent;

        public IUnit Parent
        {
            get { return mParent; }
            set { this.SetProperty(ref mParent, value); }
        }

        protected ISample mCurrentSample;
        public ISample CurrentSample
        {
            get { return mCurrentSample; }
            set { this.SetProperty(ref mCurrentSample, value); }
        }

        private bool mIsMaster;
        public bool IsMaster
        {
            get { return mIsMaster; }
            set { this.SetProperty(ref mIsMaster, value); }
        }

        public virtual void EnqueueSample(ISample sample)
        {
            mPendingQueue.Enqueue(sample);
            RaisePropertyChanged("PendingCount");
        }

        protected virtual void MoveSample()
        {
            var dest = mRouterService.FindNextDestination(this);

            dest.EnqueueSample(this.CurrentSample);
            this.CurrentSample = null;
        }

        public virtual void OnReceivedMsg(string cmd, string content)
        {
            try
            {
                //get common reply
                IResponds handler = RespondsFactory.GetRespondsHandler(cmd);
                var respMsgList = handler.GetRespondsMsg(this, content);

                foreach (var respMsg in respMsgList)
                {
                    mSendBehavior.PushMsg(respMsg);
                }
            }
            catch (Exception ex)
            {
                mLogger.LogSys(ex.Message, ex);
            }
        }

        public virtual void ResetQueue()
        {
            mPendingQueue = new ConcurrentQueue<ISample>();
        }

        protected virtual bool TryDequeueSample(out ISample sample)
        {
            return mPendingQueue.TryDequeue(out sample);
        }

        public virtual void InitUnit()
        {
            //mOwner = owner;
            if (this.GetType() != typeof(Centrifuge))
                mWaitArrivalTask = Task.Run(new Action(ProcessPendingQueue));

            mWaitArrivalTask.Start();
        }

        private void ProcessPendingQueue()
        {
            try
            {
                while (true)
                {
                    if (CurrentSample == null)
                    {
                        if (this.TryDequeueSample(out mCurrentSample))
                        {
                            OnSampleArrived();
                        }
                    }

                    Thread.Sleep(mArrivalInterval);
                }
            }
            catch (System.Exception ex)
            {
                mLogger.LogSys("ProcessPendingQueue() error", ex);
            }
        }

        protected virtual void OnSampleArrived()
        {
            var msg = SendMsg.GetMsg_1011(this, ParamConst.BCR_1);
            this.mSendBehavior.PushMsg(msg);
        }

        public UnitBase()
        {
            mChildren = new ObservableCollection<IUnit>();
            mPendingQueue = new ConcurrentQueue<ISample>();

            mLogger = ServiceLocator.Current.GetInstance<ILogService>();
            mSendBehavior = ServiceLocator.Current.GetInstance<ISendMsgBehavior>();
            mRouterService = ServiceLocator.Current.GetInstance<IRouterService>();
        }



    }
}
