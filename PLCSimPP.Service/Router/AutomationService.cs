using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BCI.PLCSimPP.Comm.Events;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Interfaces.Services;
using BCI.PLCSimPP.Comm.Models;
using BCI.PLCSimPP.Service.Devicies;
using BCI.PLCSimPP.Service.Devicies.StandardResponds;
using Prism.Events;
using Prism.Mvvm;
using GC = BCI.PLCSimPP.Service.Devicies.GC;

namespace BCI.PLCSimPP.Service.Router
{
    public class AutomationService : BindableBase, IAutomation
    {
        public const int MAX_ONLINE_COUNT = 200;
        private readonly ILogService mLogger;
        private readonly IEventAggregator mEventAggr;
        private Thread mSampleLoadingTask;
        private ConcurrentQueue<ISample> mSampleOnlineQueue;//inlet queue
        private IUnit inlet = null;
        private object mCountLocker = new object();


        public AutomationService(IPortService msgService, IRouterService router, IConfigService config, IEventAggregator eventAggregator,
                               ILogService logger, ISendMsgBehavior sender, IRecvMsgBeheavior receiver)
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

            mEventAggr.GetEvent<NotifyOffLineEvent>().Subscribe(SampleOffLine);
        }

        public bool IsConnected { get; set; }

        private ObservableCollection<IUnit> mUnitCollection;
        public ObservableCollection<IUnit> UnitCollection
        {
            get { return mUnitCollection; }
            set { SetProperty(ref mUnitCollection, value); }
        }

        public IPortService PortService { get; set; }

        public ISendMsgBehavior MsgSender { get; set; }

        public IRecvMsgBeheavior MsgReceiver { get; set; }

        public IRouterService RouterService { get; set; }

        public IConfigService ConfigService { get; set; }

        public int OnlineSampleCount { get; set; }

        public void Connect()
        {
            RouterService.SetSiteMap(UnitCollection);
            //MsgReceiver.SetUnitCollection(UnitCollection);
            PortService.Connect();
            MsgReceiver.ActiveRecvTask("");
            MsgSender.ActiveSendTask("");

            IsConnected = true;

            foreach (var port in UnitCollection)
            {
                if (port.GetType() == typeof(DynamicInlet))
                {
                    inlet = port;
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

        public void Disconnect()
        {
            PortService.Disconnect();
            IsConnected = false;
        }

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

        private void LoadingSample()
        {
            while (mSampleOnlineQueue.Count > 0)
            {
                if (OnlineSampleCount <= MAX_ONLINE_COUNT)
                {

                    bool deqflag = mSampleOnlineQueue.TryDequeue(out var sample);
                    if (deqflag)
                    {
                        inlet.EnqueueSample(sample);
                        sample.IsLoaded = true;
                        OnlineSampleCount += 1;
                    }
                }

                Thread.Sleep(1000);
            }
        }

        public void RackExchange(IUnit storckyard, string shelf, string rack)
        {
            var msg = SendMsg.GetMsg_1016(storckyard, shelf, rack);
            MsgSender.PushMsg(msg);
        }

        private void SampleOffLine(bool off)
        {
            lock (mCountLocker)
            {
                if (off)
                {
                    OnlineSampleCount += 1;
                }
            }
        }

        public void Init()
        {
            UnitCollection.Clear();
            foreach (var unit in ConfigService.ReadSiteMap())
            {
                UnitCollection.Add(unit);
            };

            //set instrument unmber
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
