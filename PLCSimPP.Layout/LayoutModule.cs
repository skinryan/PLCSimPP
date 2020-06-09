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
            regionManager.RegisterViewWithRegion(RegionName.MENU_REGION, typeof(LayoutMenu));
            regionManager.RegisterViewWithRegion(RegionName.LAYOUT_REGION, typeof(DeviceLayout));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
