using System;
using System.Collections.Generic;
using System.Text;
using PLCSimPP.Comm.Constant;
using PLCSimPP.Config.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace PLCSimPP.Config
{
    public class ConfigModule : IModule

    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(RegionName.MENUREGION, typeof(ConfigMenu));
            regionManager.RegisterViewWithRegion(RegionName.MENUREGION, typeof(AboutMenu));
            regionManager.RegisterViewWithRegion(RegionName.LAYOUTREGION, typeof(Configuration));
            regionManager.RegisterViewWithRegion(RegionName.LAYOUTREGION, typeof(About));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //throw new NotImplementedException();
        }
    }
}
