using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Events;
using BCI.PLCSimPP.Comm.Interfaces.Services;
using BCI.PLCSimPP.Comm.Models;
using BCI.PLCSimPP.Service.DB;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace BCI.PLCSimPP.Log.ViewModels
{
    public class LogMenuViewModel : BindableBase
    {
        private readonly IEventAggregator mEventAggregator;

        public ICommand NavigateCommand { get; set; }
        public ICommand ExportCommand { get; set; }

        public LogMenuViewModel(IEventAggregator eventAggr)
        {
            mEventAggregator = eventAggr;
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

            //notify a export event
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
