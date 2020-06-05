using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using BCI.PLCSimPP.Comm.Constant;
using BCI.PLCSimPP.Comm.Events;
using BCI.PLCSimPP.Comm.Interfaces.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace BCI.PLCSimPP.Config.ViewModels
{
    public class ConfigMenuViewModel : BindableBase
    {
        private readonly IEventAggregator mEventAggr;

        public ICommand NavigateCommand { get; set; }


        public ConfigMenuViewModel(IEventAggregator eventAggr)
        {
            mEventAggr = eventAggr;
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void Navigate(string viewName)
        {
            mEventAggr.GetEvent<NavigateEvent>().Publish(viewName);

        }
    }
}
