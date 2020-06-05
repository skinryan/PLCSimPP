using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using BCI.PLCSimPP.Comm;
using BCI.PLCSimPP.Comm.Events;
using BCI.PLCSimPP.Comm.Helper;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Interfaces.Services;
using BCI.PLCSimPP.Comm.Models;
using BCI.PLCSimPP.Layout.Model;
using BCI.PLCSimPP.Service.Analyzer;
using BCI.PLCSimPP.Service.Config;
using BCI.PLCSimPP.Service.Devicies;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace BCI.PLCSimPP.Layout.ViewModels
{
    public class DeviceLayoutViewModel : BindableBase
    {
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

        public ICommand ClearJammedCommand { get; set; }
        #endregion

        public IAutomation AutomationService { get; private set; }

        public DeviceLayoutViewModel(IAutomation automation, IEventAggregator eventAggr, DCSimService dcSim, DxCSimService dxcSim)
        {
            mEventAggr = eventAggr;
            AutomationService = automation;
            mDCSimService = dcSim;
            mDxCSimService = dxcSim;
            InitRackType();
            AutomationService.Init();

            SampleRangeInfo = new SampleRange() { RackType = RackType.Bypass };
            SampleCollection = new ObservableCollection<ISample>();

            ConnectCmd = new DelegateCommand(DoConnect);
            DisConnectCmd = new DelegateCommand(DoDisconnect);

            AddRangeCmd = new DelegateCommand(DoAddSample);
            DelSampleCmd = new DelegateCommand(DoClearSample);
            SaveSampleSetCmd = new DelegateCommand(DoSaveSampleSet);
            LoadSampleCmd = new DelegateCommand(DoLoadSample);
            LoadSampleSetCmd = new DelegateCommand(DoLoadSampleSet);
            RackExchangeCmd = new DelegateCommand<object>(DoRackExchange);
            ClearJammedCommand = new DelegateCommand<IUnit>(DoClearJammedSample);

            mEventAggr.GetEvent<ReLoadSiteMapEvent>().Subscribe(OnSiteMapPathChanged, ThreadOption.UIThread);
        }

        /// <summary>
        /// clear current sample
        /// </summary>
        /// <param name="unit"></param>
        private void DoClearJammedSample(IUnit unit)
        {
            //clear pending queue
            if (unit.CurrentSample != null)
            {
                unit.ResetQueue();
            }
        }

        /// <summary>
        /// reload layout
        /// </summary>
        /// <param name="needReload"></param>
        private void OnSiteMapPathChanged(bool needReload)
        {
            if (!needReload)
                return;

            DoDisconnect();
            AutomationService.Init();
        }

        /// <summary>
        /// open dc/dxc Sim
        /// </summary>
        private void StartInstrumentSim()
        {
            mDCSimService.StartUp();
            mDxCSimService.StartUp();
        }

        /// <summary>
        /// close dc/dxc Sim
        /// </summary>
        private void StopInstrumentSim()
        {
            mDCSimService.ShutDown();
            mDxCSimService.ShutDown();
        }

        private void DoRackExchange(object param)
        {
            //split rack change param 
            List<object> list = (List<object>)param;
            string floor = list[0].ToString();
            string rack = list[1].ToString();
            IUnit unit = (IUnit)list[2];

            AutomationService.RackExchange(unit, floor, rack);
        }

        private void DoLoadSample()
        {
            //find unload samples
            var unloadedSamples = from p in SampleCollection where !p.IsLoaded select p;
            AutomationService.LoadSample(unloadedSamples.ToList());
        }

        private void DoClearSample()
        {
            var result = MessageBox.Show("Confirm to clear all sample.", "Warning", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                SampleCollection.Clear();
            }
        }

        private void DoSaveSampleSet()
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Title = "Select the save path for the sample set",
                Filter = "XML File(*.xml)|*.xml",
                CheckPathExists = true,
                DefaultExt = "xml",
                RestoreDirectory = true
            };

            sfd.ShowDialog();

            //check selected file is not null
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
                MessageBox.Show("File Save Exception");
            }
        }

        private void DoLoadSampleSet()
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Select the open path for the sample set",
                Filter = "XML File(*.xml)|*.xml",
                CheckPathExists = true,
                DefaultExt = "xml",
                RestoreDirectory = true
            };

            ofd.ShowDialog();
            //check selected file is not null
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

        /// <summary>
        /// Init rack type drop down list sources
        /// </summary>
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
                AutomationService.Connect();
                ConnectedButtonEnable = false;
                StartInstrumentSim();
            }
        }

        private void DoDisconnect()
        {
            if (!ConnectedButtonEnable)
            {
                AutomationService.Disconnect();
                ConnectedButtonEnable = true;
                StopInstrumentSim();
            }
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
                Sample sample = new Sample
                {
                    SampleID = SampleRangeInfo.Characters + (SampleRangeInfo.StartNum + i).ToString().PadLeft(SampleRangeInfo.Append, '0')
                };

                //Check for duplications
                if (SampleCollection.Count(t => t.SampleID == sample.SampleID) > 0)
                {
                    continue;
                }

                sample.Rack = SampleRangeInfo.RackType;
                sample.DcToken = string.IsNullOrEmpty(SampleRangeInfo.DcToken) ? "AAA" : SampleRangeInfo.DcToken;
                sample.DxCToken = string.IsNullOrEmpty(SampleRangeInfo.DxCToken) ? "AAA" : SampleRangeInfo.DxCToken; ;
                sample.IsLoaded = false;

                SampleCollection.Add(sample);
            }
        }
    }
}
