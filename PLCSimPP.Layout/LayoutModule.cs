using System;
using System.Collections.Generic;
using System.Text;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Layout.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace BCI.PLCSimPP.Layout
{
    public class LayoutModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(RegionName.MENUREGION, typeof(LayoutMenu));
            regionManager.RegisterViewWithRegion(RegionName.LAYOUTREGION, typeof(DeviceLayout));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
