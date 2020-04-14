using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Comm.Interfaces.Services;
using PLCSimPP.Comm.Models;
using PLCSimPP.Service.Devicies;
using PLCSimPP.Service.Devicies.StandardResponds;
using GC = PLCSimPP.Service.Devicies.GC;

namespace PLCSimPP.Service.Router
{
    public class PipeLineService : IPipeLine
    {
        public const int MAX_ONLINE_COUNT = 200;
        private readonly ILogService mLogger;
        private Thread mSampleLoadingTask;
        private ConcurrentQueue<ISample> mSampleOnlineQueue;//inlet queue
        private IUnit inlet = null;

        public PipeLineService(IMsgService msgService, IRouterService router, IConfigService config,
                               ILogService logger, ISendMsgBehavior sender, IRecvMsgBeheavior receiver)
        {
            mLogger = logger;
            MsgService = msgService;
            RouterService = router;
            ConfigService = config;
            MsgSender = sender;
            MsgReceiver = receiver;

            UnitCollection = new ObservableCollection<IUnit>(ConfigService.ReadSiteMap());
            mSampleOnlineQueue = new ConcurrentQueue<ISample>();
        }

        public bool IsConnected { get; set; }

        public ObservableCollection<IUnit> UnitCollection { get; set; }

        public IAnalyzerSimBehavior AnalyzerSim { get; set; }

        public IMsgService MsgService { get; set; }

        public ISendMsgBehavior MsgSender { get; set; }

        public IRecvMsgBeheavior MsgReceiver { get; set; }

        public IRouterService RouterService { get; set; }

        public IConfigService ConfigService { get; set; }

        public int OnlineSampleCount { get; set; }

        public void Connect()
        {
            MsgReceiver.SetUnitCollection(UnitCollection);
            MsgService.Connect();
            MsgReceiver.ActiveRecvTask("");
            MsgSender.ActiveSendTask("");
            
            IsConnected = true;

            foreach (var port in UnitCollection)
            {
                if (port.GetType() == typeof(DynamicInlet))
                {
                    inlet = port;
                }
            }
        }

        public void Disconnect()
        {
            MsgService.Disconnect();
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

        public void Init()
        {
            if (UnitCollection == null)
                UnitCollection = new ObservableCollection<IUnit>();

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
        }


    }
}
