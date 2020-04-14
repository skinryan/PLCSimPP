using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using PLCSimPP.Comm;
using PLCSimPP.Comm.Events;
using PLCSimPP.Comm.Helper;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Comm.Interfaces.Services;
using PLCSimPP.Comm.Models;
using PLCSimPP.Layout.Model;
using PLCSimPP.Service.Analyzer;
using PLCSimPP.Service.Config;
using PLCSimPP.Service.Devicies;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace PLCSimPP.Layout.ViewModels
{
    public class DeviceLayoutViewModel : BindableBase
    {
        private readonly IConfigService mConfigServ;
        private readonly IEventAggregator mEventAggr;
        private readonly DCSimService mDCSimService;
        private readonly DxCSimService mDxCSimService;

        #region properties

        private bool mConnectedButtonEnable = true;
        public bool ConnectedButtonEnable
        {
            get { return mConnectedButtonEnable; }
            set { SetProperty(ref mConnectedButtonEnable, value); }
        }

        private SampleRange mSampleRangeInfo;

        public SampleRange SampleRangeInfo
        {
            get { return mSampleRangeInfo; }
            set { SetProperty(ref mSampleRangeInfo, value); }
        }

        private ObservableCollection<ISample> mSampleCollection;

        public ObservableCollection<ISample> SampleCollection
        {
            get { return mSampleCollection; }
            set { SetProperty(ref mSampleCollection, value); }
        }

        public List<RackTypeInfo> RackTypeList { get; set; }
        #endregion

        #region commands

        public ICommand AddRangeCmd { get; set; }

        public ICommand InitLayoutCommand { get; set; }

        public ICommand LoadSampleCmd { get; set; }

        public ICommand RackExchangeCmd { get; set; }

        public ICommand ConnectCmd { get; set; }

        public ICommand DisConnectCmd { get; set; }

        public ICommand SaveSampleSetCmd { get; set; }

        public ICommand DelSampleCmd { get; set; }

        public ICommand LoadSampleSetCmd { get; set; }

        //public ICommand TestCommand { get; set; }
        #endregion

        public IPipeLine PipeLineService { get; private set; }

        public DeviceLayoutViewModel(IPipeLine pipeline, IConfigService config, IEventAggregator eventAggr, DCSimService dcsim, DxCSimService dxcSim)
        {
            mEventAggr = eventAggr;
            PipeLineService = pipeline;
            mConfigServ = config;
            mDCSimService = dcsim;
            mDxCSimService = dxcSim;
            InitRackType();
            PipeLineService.Init();

            SampleRangeInfo = new SampleRange();
            SampleCollection = new ObservableCollection<ISample>();

            ConnectCmd = new DelegateCommand(DoConnect);
            DisConnectCmd = new DelegateCommand(DoDisconnect);

            AddRangeCmd = new DelegateCommand(DoAddSample);
            DelSampleCmd = new DelegateCommand(DoClearSample);
            SaveSampleSetCmd = new DelegateCommand(DoSaveSampleSet);
            LoadSampleCmd = new DelegateCommand(DoLoadSample);
            LoadSampleSetCmd = new DelegateCommand(DoLoadSampleSet);
            RackExchangeCmd = new DelegateCommand<object>(DoRackExchange);
            //TestCommand = new DelegateCommand(DoTest);

            mEventAggr.GetEvent<ReLoadSiteMapEvent>().Subscribe(OnSiteMapPathChanged);
        }

        private void OnSiteMapPathChanged(bool needReload)
        {
            if (!needReload)
                return;

            DoDisconnect();
            PipeLineService.Init();
        }

        private void StartInstrumentSim()
        {
            mDCSimService.StartUp();
            mDxCSimService.StartUp();
        }

        private void StopInstrumentSim()
        {
            mDCSimService.ShutDown();
            mDxCSimService.ShutDown();
        }

        private void DoRackExchange(object param)
        {
            List<object> list = (List<object>)param;
            string floor = list[0].ToString();
            string rack = list[1].ToString();
            IUnit unit = (IUnit)list[2];

            PipeLineService.RackExchange(unit, floor, rack);
        }

        //private void DoTest()
        //{
        //    DynamicInlet inlet = null;

        //    foreach (IUnit unit in PipeLineService.UnitCollection)
        //    {
        //        if (unit.GetType() == typeof(DynamicInlet))
        //        {
        //            inlet = (DynamicInlet)unit;
        //            break;
        //        }
        //    }

        //    inlet.EnqueueSample(new Sample() { SampleID = "11111", Rack = RackType.Remap });
        //}

        private void DoLoadSample()
        {
            var unloadedSamples = from p in SampleCollection where !p.IsLoaded select p;
            PipeLineService.LoadSample(unloadedSamples.ToList());
        }

        private void DoClearSample()
        {
            var result = MessageBox.Show("Confim to clear all sample.", "Warning", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                SampleCollection.Clear();
            }
        }

        private void DoSaveSampleSet()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Select the save path for the sample set";
            sfd.Filter = "XML File(*.xml)|*.xml";
            sfd.CheckPathExists = true;
            sfd.DefaultExt = "xml";
            sfd.RestoreDirectory = true;
            sfd.ShowDialog();

            if (string.IsNullOrEmpty(sfd.FileName))
            {
                return;
            }

            try
            {
                using (FileStream fs = new FileStream(sfd.FileName, FileMode.OpenOrCreate))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        var str = XmlConverter.SerializeISample(SampleCollection);

                        sw.Write(str);
                        sw.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("File Save Expection");
            }
        }

        private void DoLoadSampleSet()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select the open path for the sample set";
            ofd.Filter = "XML File(*.xml)|*.xml";
            ofd.CheckPathExists = true;
            ofd.DefaultExt = "xml";
            ofd.RestoreDirectory = true;
            ofd.ShowDialog();

            if (string.IsNullOrEmpty(ofd.FileName))
            {
                return;
            }

            using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    string fileContent = sr.ReadToEnd();

                    var list = XmlConverter.DeserializeISample(fileContent);

                    SampleCollection.Clear();
                    SampleCollection = new ObservableCollection<ISample>(list);
                }
            }
        }

        private void InitRackType()
        {
            RackTypeList = new List<RackTypeInfo>();

            var array = Enum.GetValues(typeof(RackType));

            foreach (RackType value in array)
            {
                RackTypeList.Add(new RackTypeInfo() { Name = EnumHelper.GetEnumDescription(value), Value = value });
            }
        }

        private void DoConnect()
        {
            if (ConnectedButtonEnable)
            {
                PipeLineService.Connect();
                ConnectedButtonEnable = false;
                StartInstrumentSim();
            }
        }

        private void DoDisconnect()
        {
            if (!ConnectedButtonEnable)
            {
                PipeLineService.Disconnect();
                ConnectedButtonEnable = true;
                StopInstrumentSim();
            }
        }

        private void LoadSample()
        {
            throw new System.NotImplementedException();
        }

        private void RackExchange()
        {
            throw new System.NotImplementedException();
        }

        private void DisConnect()
        {
            PipeLineService.Disconnect();
        }

        private void DoAddSample()
        {
            int length = 0;

            if (SampleRangeInfo.StopNum >= SampleRangeInfo.StartNum)
            {
                length = SampleRangeInfo.StopNum - SampleRangeInfo.StartNum + 1;
            }
            else
            {
                length = SampleRangeInfo.Quantity;
            }

            for (int i = 0; i < length; i++)
            {
                Sample sample = new Sample();
                sample.SampleID = SampleRangeInfo.Characters + (SampleRangeInfo.StartNum + i).ToString().PadLeft(SampleRangeInfo.Append, '0');

                //Check for duplications
                if (SampleCollection.Count(t => t.SampleID == sample.SampleID) > 0)
                {
                    continue;
                }

                sample.Rack = SampleRangeInfo.RackType;
                sample.IsLoaded = false;

                SampleCollection.Add(sample);
            }
        }

        //public void GetLayout()
        //{
        //    UnitCollection.Clear();
        //    HMOutlet hmoutlet = new HMOutlet() { Address = "0000000001", Port = 1, DisplayName = "HMOutlet" };
        //    DynamicInlet di = new DynamicInlet() { Address = "0000000002", Port = 1, DisplayName = "InletErrLane" };
        //    Centrifuge cent1 = new Centrifuge() { Address = "0000000004", Port = 1, DisplayName = "Centrifuge#1" };
        //    Centrifuge cent2 = new Centrifuge() { Address = "0000000008", Port = 1, DisplayName = "Centrifuge#2" };
        //    LevelDetector ld = new LevelDetector() { Address = "0000000010", Port = 1, DisplayName = "SerumLevel" };
        //    Labeler laber = new Labeler() { Address = "0000000020", Port = 1, DisplayName = "Labeler" };
        //    Aliquoter ali = new Aliquoter() { Address = "0000000040", Port = 1, DisplayName = "Aliquoter" };

        //    UnitCollection.Add(hmoutlet);
        //    hmoutlet.Children.Add(di);
        //    hmoutlet.Children.Add(cent1);
        //    hmoutlet.Children.Add(cent2);
        //    hmoutlet.Children.Add(ld);
        //    hmoutlet.Children.Add(laber);
        //    hmoutlet.Children.Add(ali);

        //    HLane HLane = new HLane() { Address = "0000000080", Port = 2, DisplayName = "HLane" };
        //    Service.Devicies.GC gc1 = new Service.Devicies.GC() { Address = "0000000100", Port = 2, DisplayName = "Generic #1" };
        //    Service.Devicies.GC gc2 = new Service.Devicies.GC() { Address = "0000000200", Port = 2, DisplayName = "Generic #2" };
        //    Service.Devicies.GC gc3 = new Service.Devicies.GC() { Address = "0000000400", Port = 2, DisplayName = "Generic #3" };
        //    Service.Devicies.GC gc4 = new Service.Devicies.GC() { Address = "0000000800", Port = 2, DisplayName = "Generic #4" };
        //    Service.Devicies.GC gc5 = new Service.Devicies.GC() { Address = "0000001000", Port = 2, DisplayName = "Generic #5" };
        //    Service.Devicies.GC gc6 = new Service.Devicies.GC() { Address = "0000002000", Port = 2, DisplayName = "Generic #6" };
        //    Service.Devicies.GC gc7 = new Service.Devicies.GC() { Address = "0000004000", Port = 2, DisplayName = "Generic #7" };
        //    Service.Devicies.GC gc8 = new Service.Devicies.GC() { Address = "0000008000", Port = 2, DisplayName = "Generic #8" };
        //    Service.Devicies.GC gc9 = new Service.Devicies.GC() { Address = "0000010000", Port = 2, DisplayName = "Generic #9" };
        //    Service.Devicies.GC gc10 = new Service.Devicies.GC() { Address = "0000020000", Port = 2, DisplayName = "Generic #10" };
        //    Service.Devicies.GC gc11 = new Service.Devicies.GC() { Address = "0000040000", Port = 2, DisplayName = "Generic #11" };
        //    Service.Devicies.GC gc12 = new Service.Devicies.GC() { Address = "0000080000", Port = 2, DisplayName = "Generic #12" };
        //    UnitCollection.Add(HLane);
        //    HLane.Children.Add(gc1);
        //    HLane.Children.Add(gc2);
        //    HLane.Children.Add(gc3);
        //    HLane.Children.Add(gc4);
        //    HLane.Children.Add(gc5);
        //    HLane.Children.Add(gc6);
        //    HLane.Children.Add(gc7);
        //    HLane.Children.Add(gc8);
        //    HLane.Children.Add(gc9);
        //    HLane.Children.Add(gc10);
        //    HLane.Children.Add(gc11);
        //    HLane.Children.Add(gc12);

        //    ILane ilane = new ILane() { Address = "0000100000", Port = 3, DisplayName = "ILane" };
        //    DxC dxc1 = new DxC() { Address = "0000200000", Port = 3, DisplayName = "DxC#1" };
        //    DxC dxc2 = new DxC() { Address = "0000400000", Port = 3, DisplayName = "DxC#2" };
        //    DxC dxc3 = new DxC() { Address = "0000800000", Port = 3, DisplayName = "DxC#3" };
        //    DxC dxc4 = new DxC() { Address = "0001000000", Port = 3, DisplayName = "DxC#4" };
        //    Stocker stockyard1 = new Stocker() { Address = "0002000000", Port = 3, DisplayName = "Stockyard#1" };
        //    Stocker stockyard2 = new Stocker() { Address = "0004000000", Port = 3, DisplayName = "Stockyard#2" };
        //    Stocker stockyard3 = new Stocker() { Address = "0008000000", Port = 3, DisplayName = "Stockyard#3" };
        //    Outlet outlet1 = new Outlet() { Address = "0010000000", Port = 3, DisplayName = "SigleOutlet#1" };
        //    Outlet outlet2 = new Outlet() { Address = "0020000000", Port = 3, DisplayName = "SigleOutlet#2" };
        //    UnitCollection.Add(ilane);
        //    ilane.Children.Add(dxc1);
        //    ilane.Children.Add(dxc2);
        //    ilane.Children.Add(dxc3);
        //    ilane.Children.Add(dxc4);
        //    ilane.Children.Add(stockyard1);
        //    ilane.Children.Add(stockyard2);
        //    ilane.Children.Add(stockyard3);
        //    ilane.Children.Add(outlet1);
        //    ilane.Children.Add(outlet2);
        //}

    }
}
