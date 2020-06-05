using BCI.PLCSimPP.Config.ViewDatas;
using BCI.PLCSimPP.PresentationControls;
using BCI.PLCSimPP.PresentationControls.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCI.PLCSimPP.Service.Config;
using System.Text.RegularExpressions;
using BCI.PLCSimPP.Comm.Interfaces.Services;
using CommonServiceLocator;

namespace BCI.PLCSimPP.Config.Controllers
{
    public class ConfigurationController : ControllerBase
    {
        private readonly ILogService mLogger;

        /// <summary>
        /// The data of controller
        /// </summary>     
        public ConfigurationController(ILogService logService)
        {
            mLogger = logService;
            Data = new ConfigurationViewData();
            Saving += ConfigurationControllerSaving;
            LoadViewDatasing += ConfigurationControllerViewDataLoading;
            LoadViewDatas();
        }

        private void ConfigurationControllerSaving()
        {
            try
            {
                if (!(Data is ConfigurationViewData data))
                    return;

                var configService = ServiceLocator.Current.GetInstance<IConfigService>();
                var configInfo = new Comm.Models.SystemInfo
                {
                    SiteMapPath = data.SiteMapFilePath,
                    DcSimLocation = data.DcSimLocation,
                    DcInstruments = data.AnalyzerItems,
                    DxCSimLocation = data.DxCSimLocation,
                    DxCInstruments = data.DxCAnalyzerItems,
                    SendInterval = data.SendInterval
                };

                configService.SaveSysConfig(configInfo);
            }
            catch (Exception ex)
            {
                mLogger.LogSys("Exception : ConfigurationController.ConfigurationControllerSaving - " + ex);
            }
        }

        private void ConfigurationControllerViewDataLoading()
        {
            try
            {
                if (!(Data is ConfigurationViewData data))
                    return;

                var configService = ServiceLocator.Current.GetInstance<IConfigService>();
                var config = configService.ReadSysConfig();
                data.SiteMapFilePath = config.SiteMapPath;
                data.SendInterval = config.SendInterval;
                data.DxCSimLocation = config.DxCSimLocation;
                data.DxCAnalyzerItems = config.DxCInstruments;
                data.DcSimLocation = config.DcSimLocation;
                data.AnalyzerItems = config.DcInstruments;
            }
            catch (Exception ex)
            {
                mLogger.LogSys("Exception : ConfigurationController.ConfigurationControllerViewDataLoading - " + ex);
            }
        }
    }
}
