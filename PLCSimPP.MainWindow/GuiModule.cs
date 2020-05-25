using System;
using System.Collections.Generic;
using System.Text;
using BCI.PLCSimPP.Comm.Constant;
using BCI.PLCSimPP.MainWindow.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace BCI.PLCSimPP.MainWindow
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
