using System;
using System.Collections.Generic;
using System.Text;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Comm.Interfaces.Services;
using PLCSimPP.Service.Analyzer;
using PLCSimPP.Service.Config;
using PLCSimPP.Service.Console;
using PLCSimPP.Service.Log;
using PLCSimPP.Service.Msg;
using PLCSimPP.Service.Router;
using PLCSimPP.Service.Services;
using Prism.Ioc;
using Prism.Modularity;

namespace PLCSimPP.Service
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
            containerRegistry.RegisterSingleton<IConsoleService, ConsoleService>();
            containerRegistry.RegisterSingleton<ILogService, LogService>();
            containerRegistry.RegisterSingleton<IConfigService, ConifgService>();
            containerRegistry.RegisterSingleton<IRouterService, RouterService>();
            containerRegistry.RegisterSingleton<IRecvMsgBeheavior, MsgReceiver>();
            containerRegistry.RegisterSingleton<IMsgService, MsgService>();
            containerRegistry.RegisterSingleton<ISendMsgBehavior, MsgSender>();
            containerRegistry.RegisterSingleton<IPipeLine, PipeLineService>();
            containerRegistry.RegisterSingleton<DCSimService>();
            containerRegistry.RegisterSingleton<DxCSimService>();
        }
    }
}
