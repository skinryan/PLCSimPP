using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using PLCSimPP.Comm.Constant;
using PLCSimPP.Comm.Events;
using PLCSimPP.Comm.Interfaces.Services;
using PLCSimPP.Comm.Models;
using PLCSimPP.Service.DB;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace PLCSimPP.Log.ViewModels
{
    public class LogMenuViewModel : BindableBase
    {
        private readonly ILogService mLogServ;
        private readonly IEventAggregator mEventAggregator;

        public ICommand NavigateCommand { get; set; }
        public ICommand ExportCommand { get; set; }

        public LogMenuViewModel(IEventAggregator eventAggr, ILogService logServ)
        {
            mEventAggregator = eventAggr;
            mLogServ = logServ;
            NavigateCommand = new DelegateCommand<string>(Navigate);
            ExportCommand = new DelegateCommand(DoExport);
        }

        private void DoExport()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Select the export path";
            sfd.Filter = "CSV file (*.csv)|*.csv";
            sfd.CheckPathExists = true;
            sfd.DefaultExt = "csv";
            sfd.RestoreDirectory = true;
            sfd.ShowDialog();

            //var path = sfd.FileName.Substring(0, sfd.FileName.LastIndexOf('\\'));
            //var name = sfd.FileName.Substring(sfd.FileName.LastIndexOf('\\') + 1);
            //LogDB.Current.BackupDb(path, name);
            if (!string.IsNullOrEmpty(sfd.FileName))
            {
                mEventAggregator.GetEvent<ExportEvent>().Publish(sfd.FileName);
            }
        }

        private void Navigate(string viewName)
        {

            mEventAggregator.GetEvent<NavigateEvent>().Publish(viewName);

        }
    }
}
