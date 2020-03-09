using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using PLCSimPP.Comm.Constant;
using PLCSimPP.Comm.Interfaces.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace PLCSimPP.Layout.ViewModels
{
    public class LayoutMenuViewModel : BindableBase
    {
        private readonly ILogService mLogServ;
        private readonly IRegionManager mRegionManager;

        public ICommand NavigateCommand { get; set; }


        public LayoutMenuViewModel(IRegionManager region, ILogService logServ)
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
