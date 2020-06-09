using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Config.ViewModels;
using BCI.PLCSimPP.Config.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace BCI.PLCSimPP.Config
{
    public class ConfigModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(RegionName.MENU_REGION, typeof(ConfigMenu));
            regionManager.RegisterViewWithRegion(RegionName.MENU_REGION, typeof(AboutMenu));
            regionManager.RegisterViewWithRegion(RegionName.LAYOUT_REGION, typeof(Configuration));
            regionManager.RegisterViewWithRegion(RegionName.LAYOUT_REGION, typeof(SiteMapEditer));
            regionManager.RegisterViewWithRegion(RegionName.LAYOUT_REGION, typeof(About));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.RegisterSingleton<INotifyPropertyChanged, ConfigurationViewModel>("ConfigurationViewModel");
        }
    }
}
