using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using PLCSimPP.Comm.Constant;
using PLCSimPP.Comm.Interfaces.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace PLCSimPP.Config.ViewModels
{
    public class AboutMenuViewModel : BindableBase
    {
        private readonly ILogService mLogServ;
        private readonly IRegionManager mRegionManager;

        public ICommand NavigateCommand { get; set; }


        public AboutMenuViewModel(IRegionManager region, ILogService logServ)
        {
            mRegionManager = region;
            mLogServ = logServ;
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void Navigate(string viewName)
        {

            mRegionManager.RequestNavigate(RegionName.LAYOUTREGION, viewName);

        }
    }
}
