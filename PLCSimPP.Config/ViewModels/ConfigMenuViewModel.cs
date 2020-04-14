using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using PLCSimPP.Comm.Constant;
using PLCSimPP.Comm.Events;
using PLCSimPP.Comm.Interfaces.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace PLCSimPP.Config.ViewModels
{
    public class ConfigMenuViewModel : BindableBase
    {
        private readonly ILogService mLogServ;
        private readonly IEventAggregator mEventAggr;

        public ICommand NavigateCommand { get; set; }


        public ConfigMenuViewModel(IEventAggregator eventAggr, ILogService logServ)
        {
            mEventAggr = eventAggr;
            mLogServ = logServ;
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void Navigate(string viewName)
        {
            mEventAggr.GetEvent<NavigateEvent>().Publish(viewName);

        }
    }
}
