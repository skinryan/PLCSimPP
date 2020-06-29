using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Events;
using BCI.PLCSimPP.Comm.Interfaces.Services;
using BCI.PLCSimPP.Config.ViewModels;
using BCI.PLCSimPP.Config.Views;
using BCI.PLCSimPP.Service.DB;
using CommonServiceLocator;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace BCI.PLCSimPP.MainWindow.ViewModels
{
    public class FrameViewModel : BindableBase
    {
        private const string EXPORT_COMPLETE = "Export complete.";
        private const string EXPORTING = "Exporting...";
        private const string CSV_HEADER = "Time,Direction,Address,Command,Details";
        private readonly IRegionManager mRegionManager;
        private readonly IEventAggregator mEventAggr;
        private readonly BackgroundWorker mBgWorker;
        private string mFilePath;

        #region properties

        private bool mMenuBarVisibility = true;

        public bool MenuBarVisibility
        {
            get { return mMenuBarVisibility; }
            set { SetProperty(ref mMenuBarVisibility, value); }
        }

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
            mBgWorker.DoWork += BackgroundWorker_DoWork;

            mEventAggr.GetEvent<NavigateEvent>().Subscribe(OnNavigate);
            mEventAggr.GetEvent<ExportEvent>().Subscribe(OnExport);
            mEventAggr.GetEvent<ConnectionStatusEvent>().Subscribe(OnConnectionStatusChanged);
            mEventAggr.GetEvent<NotifyPortCountEvent>().Subscribe(OnPortCountChanged);
        }

        private void OnPortCountChanged(int count)
        {
            //update port3 status
            Port3Enabled = count > 2;
        }

        private void OnConnectionStatusChanged(ConnInfo connInfo)
        {
            switch (connInfo.Port)
            {
                case 1:
                    Port1Status = connInfo.IsConnected;
                    break;
                case 2:
                    Port2Status = connInfo.IsConnected;
                    break;
                case 3:
                    Port3Status = connInfo.IsConnected;
                    break;
                default:
                    return;
            }
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // export csv file
            var logs = DBService.Current.QueryLogContents();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(CSV_HEADER);

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

            BackWorkerStatus = EXPORT_COMPLETE;
            Thread.Sleep(3000);
            BackWorkerStatus = string.Empty;
        }

        private void OnExport(string exportPath)
        {
            mFilePath = exportPath;
            BackWorkerStatus = EXPORTING;
            mBgWorker.RunWorkerAsync();
        }

        private void OnNavigate(string viewName)
        {
            var views = mRegionManager.Regions[RegionName.LAYOUT_REGION].ActiveViews;
            var activeView = views.FirstOrDefault();

            if (viewName != ViewName.SITE_MAP_EDITER)
            {
                MenuBarVisibility = true;

                if (activeView is Configuration configuration)//check config is edited
                {
                    if (viewName == ViewName.CONFIGURATION)
                    {
                        return;
                    }

                    var configVm = configuration.ViewModel;
                    var ret = configVm.CheckLeaving();
                    if (!ret)
                    {
                        return;
                    }
                }
            }
            else
            {
                MenuBarVisibility = false;

                if (activeView is SiteMapEditer editer)
                {
                    var siteMapVm = editer.ViewModel;
                    var ret = siteMapVm.Leaving();
                    if (!ret)
                    {
                        return;
                    }
                }

            }

            mRegionManager.RequestNavigate(RegionName.LAYOUT_REGION, viewName);

            if (RegionName.ViewName.ContainsKey(viewName))
            {
                var title = RegionName.ViewName[viewName];
                mEventAggr.GetEvent<SetTitleEvent>().Publish(title);
            }
        }
    }


}
