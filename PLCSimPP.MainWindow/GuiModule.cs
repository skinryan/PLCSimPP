using System;
using System.Collections.Generic;
using System.Text;
using PLCSimPP.Comm.Constant;
using PLCSimPP.MainWindow.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace PLCSimPP.MainWindow
{
    public class MainWindowModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(RegionName.CONTENTREGION, typeof(Frame));
            
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
