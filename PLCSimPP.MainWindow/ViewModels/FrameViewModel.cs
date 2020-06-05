﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BCI.PLCSimPP.Comm.Constant;
using BCI.PLCSimPP.Comm.Events;
using BCI.PLCSimPP.Comm.Interfaces.Services;
using BCI.PLCSimPP.Service.DB;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace BCI.PLCSimPP.MainWindow.ViewModels
{
    public class FrameViewModel : BindableBase
    {
        private IRegionManager mRegionManager;
        private IEventAggregator mEventAggr;
        private BackgroundWorker mBgWorker;
        private string mFilePath;

        #region properties

        private string mBackWorkerStatus;

        public string BackWorkerStatus
        {
            get { return mBackWorkerStatus; }
            set { SetProperty(ref mBackWorkerStatus, value); }
        }

        private bool mPort1Status;

        public bool Port1Status
        {
            get { return mPort1Status; }
            set { SetProperty(ref mPort1Status, value); }
        }

        private bool mPort2Status;

        public bool Port2Status
        {
            get { return mPort2Status; }
            set { SetProperty(ref mPort2Status, value); }
        }

        private bool mPort3Status;

        public bool Port3Status
        {
            get { return mPort3Status; }
            set { SetProperty(ref mPort3Status, value); }
        }

        private bool mPort3Enabled;

        public bool Port3Enabled
        {
            get { return mPort3Enabled; }
            set { SetProperty(ref mPort3Enabled, value); }
        }

        #endregion

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="region">RegionManager</param>
        /// <param name="eventAggr">EventAggregator</param>
        public FrameViewModel(IRegionManager region, IEventAggregator eventAggr)
        {
            mRegionManager = region;
            mEventAggr = eventAggr;

            mBgWorker = new BackgroundWorker();
            mBgWorker.DoWork += MBgWorker_DoWork;

            mEventAggr.GetEvent<NavigateEvent>().Subscribe(OnNavigate);
            mEventAggr.GetEvent<ExportEvent>().Subscribe(OnExport);
            mEventAggr.GetEvent<ConnectionStatusEvent>().Subscribe(OnConnectionStatusChanged);
            mEventAggr.GetEvent<NotifyPortCountEvent>().Subscribe(OnPortCountChanged);
        }
        
        private void OnPortCountChanged(int count)
        {
            if (count > 2)
            {
                Port3Enabled = true;
            }
            else
            {
                Port3Enabled = false;
            }
        }

        private void OnConnectionStatusChanged(ConnInfo connInfo)
        {
            if (connInfo.Port == 1)
            {
                Port1Status = connInfo.IsConnected;
            }

            if (connInfo.Port == 2)
            {
                Port2Status = connInfo.IsConnected;
            }

            if (connInfo.Port == 3)
            {
                Port3Status = connInfo.IsConnected;
            }
        }
        
        private void MBgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //do export csv file
            var logs = DBService.Current.QueryLogContents();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Time,Direction,Address,Command,Details");

            foreach (var log in logs)
            {
                sb.AppendLine($"{log.Time},{log.Direction},{log.Address},{log.Command},{log.Details}");
            }

            using (FileStream fs = new FileStream(mFilePath, FileMode.OpenOrCreate))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(sb.ToString());
                    sw.Flush();
                }
            }

            BackWorkerStatus = "Export complete";
            Thread.Sleep(3000);
            BackWorkerStatus = string.Empty;
        }

        private void OnExport(string exportPath)
        {
            mFilePath = exportPath;
            BackWorkerStatus = "Exporting...";
            //invoked asynchronously
            mBgWorker.RunWorkerAsync();
        }

        private void OnNavigate(string viewName)
        {
            mRegionManager.RequestNavigate(RegionName.LAYOUTREGION, viewName);

            if (RegionName.ViewName.ContainsKey(viewName))
            {
                var title = RegionName.ViewName[viewName];

                mEventAggr.GetEvent<SetTitleEvent>().Publish(title);
            }
        }
    }
}
