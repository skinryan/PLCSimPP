using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Events;
using BCI.PLCSimPP.Comm.Interfaces.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace BCI.PLCSimPP.Layout.ViewModels
{
    public class LayoutMenuViewModel : BindableBase
    {
        private readonly ILogService mLogServ;
        private readonly IEventAggregator mEventAggr;

        public ICommand NavigateCommand { get; set; }


        public LayoutMenuViewModel(IEventAggregator eventAggr, ILogService logServ)
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
