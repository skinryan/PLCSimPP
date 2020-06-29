using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BCI.PLCSimPP.Comm.Events;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Interfaces.Services;
using BCI.PLCSimPP.Comm.Models;
using BCI.PLCSimPP.Service.Devices;
using BCI.PLCSimPP.Service.Devices.StandardResponds;
using Prism.Events;
using Prism.Mvvm;
using GC = BCI.PLCSimPP.Service.Devices.GC;

namespace BCI.PLCSimPP.Service.Router
{

    public class AutomationService : BindableBase, IAutomation
    {
        public const int MAX_ONLINE_COUNT = 200;
        private readonly ILogService mLogger;
        private readonly IEventAggregator mEventAggr;
        private Thread mSampleLoadingTask;
        private readonly ConcurrentQueue<ISample> mSampleOnlineQueue;//inlet queue
        private IUnit mInlet = null;
        private readonly object mCountLocker = new object();

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="msgService">message service</param>
        /// <param name="router">router service</param>
        /// <param name="config">configuration service</param>
        /// <param name="eventAggregator">event aggregator</param>
        /// <param name="logger">logger service</param>
        /// <param name="sender">message send service</param>
        /// <param name="receiver">message receive service</param>
        public AutomationService(IPortService msgService, IRouterService router, IConfigService config, IEventAggregator eventAggregator,
                               ILogService logger, ISendMsgBehavior sender, IRecvMsgBehavior receiver)
        {
            mLogger = logger;
            PortService = msgService;
            RouterService = router;
            ConfigService = config;
            MsgSender = sender;
            MsgReceiver = receiver;
            mEventAggr = eventAggregator;

            UnitCollection = new ObservableCollection<IUnit>(ConfigService.ReadSiteMap());
            mSampleOnlineQueue = new ConcurrentQueue<ISample>();

            mEventAggr.GetEvent<NotifyOnlineSampleEvent>().Subscribe(OnSampleCountChanged);
        }

        /// <summary>
        /// system connection flag
        /// </summary>
        public bool IsConnected { get; set; }

        /// <summary>
        /// port service
        /// </summary>
        public IPortService PortService { get; set; }

        /// <summary>
        /// message send behavior
        /// </summary>
        public ISendMsgBehavior MsgSender { get; set; }

        /// <summary>
        /// message receive behavior
        /// </summary>
        public IRecvMsgBehavior MsgReceiver { get; set; }

        /// <summary>
        /// router service
        /// </summary>
        public IRouterService RouterService { get; set; }

        /// <summary>
        /// configuration service
        /// </summary>
        public IConfigService ConfigService { get; set; }

        #region Bindable properties

        private ObservableCollection<IUnit> mUnitCollection;
        /// <summary>
        /// system layout units
        /// </summary>
        public ObservableCollection<IUnit> UnitCollection
        {
            get { return mUnitCollection; }
            set { SetProperty(ref mUnitCollection, value); }
        }

        private int mOnlineSampleCount;
        /// <summary>
        /// sample online count
        /// </summary>
        public int OnlineSampleCount
        {
            get { return mOnlineSampleCount; }
            set { SetProperty(ref mOnlineSampleCount, value); }
        }

        #endregion

        /// <summary>
        /// Start port server and init units and active send/receive task
        /// </summary>
        public void Connect()
        {
            RouterService.SetSiteMap(UnitCollection);
            PortService.Connect();
            MsgReceiver.ActiveRecvTask("");
            MsgSender.ActiveSendTask("");

            IsConnected = true;

            foreach (var port in UnitCollection)
            {
                if (port.GetType() == typeof(DynamicInlet))
                {
                    mInlet = port;
                }

                port.InitUnit();

                if (port.HasChild)
                {
                    foreach (var device in port.Children)
                    {
                        device.InitUnit();
                    }
                }
            }
        }

        /// <summary>
        /// stop port server
        /// </summary>
        public void Disconnect()
        {
            PortService.Disconnect();
            IsConnected = false;
        }

        /// <inheritdoc />
       
        public void LoadSample(List<ISample> samples)
        {
            foreach (var sample in samples)
            {
                if (!mSampleOnlineQueue.Contains(sample))
                {
                    mSampleOnlineQueue.Enqueue(sample);
                }
            }

            ActiveLoadingTask();
        }

        /// <summary>
        /// active loading sample task
        /// </summary>
        private void ActiveLoadingTask()
        {
            if (mSampleLoadingTask == null)
                return;

            if (mSampleLoadingTask.ThreadState == ThreadState.Stopped)
            {
                mSampleLoadingTask = new Thread(new ThreadStart(LoadingSample));
            }

            if (mSampleLoadingTask.ThreadState == ThreadState.Unstarted)
            {
                mSampleLoadingTask.Start();
            }
        }

        /// <summary>
        /// load sample action
        /// </summary>
        private void LoadingSample()
        {
            while (mSampleOnlineQueue.Count > 0)
            {
                if (OnlineSampleCount <= MAX_ONLINE_COUNT)
                {
                    bool deqflag = mSampleOnlineQueue.TryDequeue(out var sample);
                    if (deqflag)
                    {
                        mInlet.EnqueueSample(sample);
                        sample.IsLoaded = true;

                        mEventAggr.GetEvent<NotifyOnlineSampleEvent>().Publish(1);
                    }
                }

                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// rack exchange
        /// </summary>
        /// <param name="stockyard"></param>
        /// <param name="shelf"></param>
        /// <param name="rack"></param>
        public void RackExchange(IUnit stockyard, string shelf, string rack)
        {
            var msg = SendMsg.GetMsg1016(stockyard, shelf, rack);

            this.mEventAggr.GetEvent<NotifyRackExchangeEvent>().Publish(new RackExchangeParam()
            {
                Address = stockyard.Address,
                Rack = rack,
                Shelf = shelf
            });
            MsgSender.PushMsg(msg);
        }

        /// <summary>
        /// OnSampleCountChanged
        /// </summary>
        /// <param name="flag"> 1:online, -1:offline, 0:reset</param>
        private void OnSampleCountChanged(int flag)
        {
            lock (mCountLocker)
            {
                if (flag < 0)
                {
                    OnlineSampleCount -= 1;
                }
                else if (flag > 0)
                {
                    OnlineSampleCount += 1;
                }
            }
        }

        /// <inheritdoc />
        public void Init()
        {
            UnitCollection.Clear();
            foreach (var unit in ConfigService.ReadSiteMap())
            {
                UnitCollection.Add(unit);
            };

            //set instrument number
            var dcCount = 1;
            var dxcCount = 1;
            foreach (var unit in UnitCollection)
            {
                if (unit.HasChild)
                {
                    unit.IsMaster = true;
                    foreach (var subUnit in unit.Children)
                    {
                        if (subUnit.GetType() == typeof(GC))
                        {
                            ((GC)subUnit).InstrumentUnitNum = dcCount;
                            dcCount += 1;
                            continue;
                        }

                        if (subUnit.GetType() == typeof(DxC))
                        {
                            ((DxC)subUnit).InstrumentUnitNum = dxcCount;
                            dxcCount += 1;
                            continue;
                        }
                    }
                }
            }

            this.Disconnect();
            mSampleLoadingTask = new Thread(new ThreadStart(LoadingSample));
            mEventAggr.GetEvent<NotifyPortCountEvent>().Publish(UnitCollection.Count);
        }


    }
}
