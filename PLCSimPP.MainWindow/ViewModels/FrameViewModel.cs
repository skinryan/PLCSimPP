using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PLCSimPP.Comm.Constant;
using PLCSimPP.Comm.Events;
using PLCSimPP.Comm.Interfaces.Services;
using PLCSimPP.Service.DB;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace PLCSimPP.MainWindow.ViewModels
{
    public class FrameViewModel : BindableBase
    {
        private ILogService mLogger;
        private IRegionManager mRegionManager;
        private IEventAggregator mEventAggr;
        private BackgroundWorker mBgWorker;
        private string mFilePath;

        private string mBackWorkerStatus;

        public string BackWorkerStatus
        {
            get { return mBackWorkerStatus; }
            set { SetProperty(ref mBackWorkerStatus, value); }
        }

        public DelegateCommand ChangeTitleCommand { get; private set; }

        public FrameViewModel(ILogService LoggerService, IRegionManager region, IEventAggregator eventAggr)
        {
            mLogger = LoggerService;

            mRegionManager = region;
            mEventAggr = eventAggr;

            mBgWorker = new BackgroundWorker();
            mBgWorker.DoWork += MBgWorker_DoWork;


            mEventAggr.GetEvent<NavigateEvent>().Subscribe(OnNavigate);
            mEventAggr.GetEvent<ExportEvent>().Subscribe(OnExport);
        }

        private void MBgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var logs = LogDB.Current.QueryLogContents();
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
