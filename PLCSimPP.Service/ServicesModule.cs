using System;
using System.Collections.Generic;
using System.Text;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Interfaces.Services;
using BCI.PLCSimPP.Service.Analyzer;
using BCI.PLCSimPP.Service.Config;
using BCI.PLCSimPP.Service.Log;
using BCI.PLCSimPP.Service.Msg;
using BCI.PLCSimPP.Service.Router;
using BCI.PLCSimPP.Service.Services;
using Prism.Ioc;
using Prism.Modularity;

namespace BCI.PLCSimPP.Service
{
    public class ServicesModule : IModule
    {
        public ServicesModule()
        {

        }

        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ILogService, LogService>();
            containerRegistry.RegisterSingleton<IConfigService, ConfigService>();
            containerRegistry.RegisterSingleton<IRouterService, RouterService>();
            containerRegistry.RegisterSingleton<IRecvMsgBehavior, MsgReceiver>();
            containerRegistry.RegisterSingleton<IPortService, PortService>();
            containerRegistry.RegisterSingleton<ISendMsgBehavior, MsgSender>();
            containerRegistry.RegisterSingleton<IAutomation, AutomationService>();
            containerRegistry.RegisterSingleton<IAnalyzerSimService, DCSimService>("DCSimService");
            containerRegistry.RegisterSingleton<IAnalyzerSimService, DxCSimService>("DxCSimService");
        }
    }
}
