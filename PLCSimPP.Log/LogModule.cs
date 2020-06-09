using System;
using System.Collections.Generic;
using System.Text;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Log.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace BCI.PLCSimPP.Log
{
    public class LogModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(RegionName.MENU_REGION, typeof(LogMenu));
            regionManager.RegisterViewWithRegion(RegionName.LAYOUT_REGION, typeof(LogViewer));
            regionManager.RegisterViewWithRegion(RegionName.LAYOUT_REGION, typeof(Monitor));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
