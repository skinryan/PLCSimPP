using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using CommonServiceLocator;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Interfaces.Services;
using BCI.PLCSimPP.Service.Devices.StandardResponds;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm;
using Prism.Mvvm;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading;
using BCI.PLCSimPP.Comm.Models;
using BCI.PLCSimPP.Comm.Events;
using Prism.Events;

namespace BCI.PLCSimPP.Service.Devices
{
    /// <summary>
    /// UnitBase
    /// </summary>
    public abstract class UnitBase : BindableBase, IUnit
    {
        protected readonly ISendMsgBehavior mSendBehavior;
        protected readonly ILogService mLogger;
        protected readonly IRouterService mRouterService;
        protected readonly IEventAggregator mEventAggr;
        protected ConcurrentQueue<ISample> mPendingQueue;
        protected Task mWaitArrivalTask;
        protected int mArrivalInterval = 1000;

        #region properties

        private string mDisplayName;
        /// <summary>
        /// DisplayName
        /// </summary>
        public string DisplayName
        {
            get { return mDisplayName; }
            set { SetProperty(ref mDisplayName, value); }
        }

        private int mPort;
        /// <summary>
        /// Port No.
        /// </summary>
        public int Port
        {
            get { return mPort; }
            set { SetProperty(ref mPort, value); }
        }

        private string mAddress;
        /// <summary>
        /// Address 
        /// </summary>
        public string Address
        {
            get { return mAddress; }
            set { this.SetProperty(ref mAddress, value); }
        }

        /// <summary>
        /// Port children
        /// </summary>
        public ObservableCollection<IUnit> Children
        {
            get;
        }

        /// <summary>
        /// The length of the queue to be processed by the unit
        /// </summary>
        public int PendingCount
        {
            get { return GetPendingCount(); }
        }

        /// <summary>
        /// Mark the unit has child
        /// </summary>
        public bool HasChild
        {
            get { return Children.Count > 0; }
        }

        private IUnit mParent;
        /// <summary>
        /// Mark the unit parent
        /// </summary>
        public IUnit Parent
        {
            get { return mParent; }
            set { this.SetProperty(ref mParent, value); }
        }

        protected ISample mCurrentSample;
        /// <summary>
        /// Sample in the current process
        /// </summary>
        public ISample CurrentSample
        {
            get { return mCurrentSample; }
            set { this.SetProperty(ref mCurrentSample, value); }
        }

        private bool mIsMaster;
        /// <summary>
        /// Mark the unit is master port
        /// </summary>
        public bool IsMaster
        {
            get { return mIsMaster; }
            set { this.SetProperty(ref mIsMaster, value); }
        }

        #endregion
        /// <summary>
        /// Get pending sample count
        /// </summary>
        /// <returns></returns>
        protected virtual int GetPendingCount()
        {
            return mPendingQueue.Count;
        }

        /// <summary>
        /// Enqueue sample from pending queue
        /// </summary>
        /// <param name="sample"></param>
        public virtual void EnqueueSample(ISample sample)
        {
            mPendingQueue.Enqueue(sample);
            RaisePropertyChanged("PendingCount");
        }

        /// <summary>
        /// MoveSample
        /// </summary>
        protected virtual void MoveSample()
        {
            var dest = mRouterService.FindNextDestination(this);

            dest.EnqueueSample(this.CurrentSample);
            this.CurrentSample = null;
            RaisePropertyChanged("PendingCount");
        }

        /// <inheritdoc />
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

        /// <summary>
        /// skip one sample
        /// </summary>
        public virtual void ResetQueue()
        {
            this.CurrentSample = null;
            RaisePropertyChanged("PendingCount");

            var eventAggr = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggr.GetEvent<NotifyOnlineSampleEvent>().Publish(-1);
        }

        /// <summary>
        /// get sample from pending queue
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        protected virtual bool TryDequeueSample(out ISample sample)
        {
            return mPendingQueue.TryDequeue(out sample);
        }

        /// <summary>
        /// init
        /// </summary>
        public virtual void InitUnit()
        {
            //mOwner = owner;
            if (this.GetType() != typeof(Centrifuge))
            {
                mWaitArrivalTask = new Task(ProcessPendingQueue);
                mWaitArrivalTask.Start();
            }
        }

        /// <summary>
        /// Process Pending Queue
        /// </summary>
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

        /// <summary>
        /// On Sample Arrived
        /// </summary>
        protected virtual void OnSampleArrived()
        {
            var msg = SendMsg.GetMsg1011(this, ParamConst.BCR_1);
            this.mSendBehavior.PushMsg(msg);
        }

        /// <summary>
        /// constructor
        /// </summary>
        protected UnitBase()
        {
            Children = new ObservableCollection<IUnit>();
            mPendingQueue = new ConcurrentQueue<ISample>();

            if (ServiceLocator.IsLocationProviderSet)
            {
                mLogger = ServiceLocator.Current.GetInstance<ILogService>();
                mSendBehavior = ServiceLocator.Current.GetInstance<ISendMsgBehavior>();
                mRouterService = ServiceLocator.Current.GetInstance<IRouterService>();
                mEventAggr = ServiceLocator.Current.GetInstance<IEventAggregator>();
            }
        }
    }
}
