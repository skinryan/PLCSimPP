using PLCSimPP.Config.ViewDatas;
using PLCSimPP.PresentationControls;
using PLCSimPP.PresentationControls.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLCSimPP.Service.Config;
using System.Text.RegularExpressions;
using PLCSimPP.Comm.Interfaces.Services;
using CommonServiceLocator;

namespace PLCSimPP.Config.Controllers
{
    public class ConfigurationController : ControllerBase
    {
        private const int MIN_SEND_INTERVAL = 0;
        private const int MAX_SEND_INTERVAL = 10000;

        /// <summary>
        /// The data of controller
        /// </summary>     
        public ConfigurationController()
        {
            Data = new ConfigruationViewData();
            Saving += ConfigurationControllerSaving;
            LoadViewDatasing += ConfigurationControllerViewDataLoading;
            LoadViewDatas();
        }
        void ConfigurationControllerSaving()
        {

            try
            {
                var data = Data as ConfigruationViewData;
                if (data == null)
                    return;
                var configService = ServiceLocator.Current.GetInstance<IConfigService>();
                var configInfo = new Comm.Models.SystemInfo();

                configInfo.SiteMapPath = data.SitemapFilePath;
                configInfo.DcSimLocation = data.DcSimLocation;
                configInfo.DcInstruments = data.AnalyzerItems;
                configInfo.DxCSimLocation = data.DxCSimLocation;
                configInfo.DxCInstruments = data.DxCAnalyzerItems;
                configInfo.SendInterval = data.SendInterval;

                //if (NumberRange.IsValid(data.SendInterval, MIN_SEND_INTERVAL, MAX_SEND_INTERVAL))
                //{
                //    configInfo.MsgReceiveInterval = data.SendInterval;
                //}
                configService.SaveSysConfig(configInfo);
            }
            catch (Exception ex)
            {

                //mLog.Error("Exception : ConfigurationController.ConfigurationControllerSaving - " + ex);
            }
        }
        void ConfigurationControllerViewDataLoading()
        {
            try
            {
                var data = Data as ConfigruationViewData;
                if (data == null)
                    return;
                var configService = ServiceLocator.Current.GetInstance<IConfigService>();
                var config = configService.ReadSysConfig();
                data.SitemapFilePath = config.SiteMapPath;
                data.SendInterval = config.SendInterval;
                data.DxCSimLocation = config.DxCSimLocation;
                data.DxCAnalyzerItems = config.DxCInstruments;
                data.DcSimLocation = config.DcSimLocation;
                data.AnalyzerItems = config.DcInstruments;
            }
            catch (Exception ex)
            {
                //mLog.Error("Exception : ConfigurationController.ConfigurationControllerViewDataLoading - " + ex);
            }
        }
    }
}
