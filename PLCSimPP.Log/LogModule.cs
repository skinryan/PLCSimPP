using System;
using System.Collections.Generic;
using System.Text;
using PLCSimPP.Comm.Constant;
using PLCSimPP.Log.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace PLCSimPP.Log
{
    public class LogModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(RegionName.MENUREGION, typeof(LogMenu));
            regionManager.RegisterViewWithRegion(RegionName.LAYOUTREGION, typeof(LogViewer));
            regionManager.RegisterViewWithRegion(RegionName.LAYOUTREGION, typeof(Monitor));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
