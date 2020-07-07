using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DcSimCom;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Interfaces.Services;
using BCI.PLCSimPP.Comm.Models;
using Prism.Events;
using BCI.PLCSimPP.Comm.Events;

namespace BCI.PLCSimPP.Service.Analyzer
{
    /// <summary>
    /// DCSim Service
    /// </summary>
    public class DCSimService : IAnalyzerSimService
    {
        private readonly IConfigService mConfigService;
        private readonly IEventAggregator mEventAggr;
        private SystemInfo mConfig;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="config"></param>
        /// <param name="eventAggr"></param>
        public DCSimService(IConfigService config, IEventAggregator eventAggr)
        {
            mConfigService = config;
            mEventAggr = eventAggr;
            mConfig = mConfigService.ReadSysConfig();
            mEventAggr.GetEvent<ReLoadSiteMapEvent>().Subscribe(OnReload);
        }

        /// <summary>
        /// reload system config
        /// </summary>
        /// <param name="obj"></param>
        private void OnReload(bool obj)
        {
            mConfig = mConfigService.ReadSysConfig();
        }

        /// <summary>
        /// send message to DCSim
        /// </summary>
        /// <param name="unitNum"></param>
        /// <param name="token"></param>
        /// <param name="sampleId"></param>
        public void SendMsg(int unitNum, string token, string sampleId)
        {
            LoadSampleOnAnalyzerMessage analyzerMsg = new LoadSampleOnAnalyzerMessage()
            {
                DInstrTokenId = token,
                SampleId = sampleId,
                UnitNumber = unitNum
            };
            CommServerForDcSim.Instance.Send(analyzerMsg);
        }

        /// <summary>
        /// Shut down DCSim
        /// </summary>
        public void ShutDown()
        {
            var dcSimPath = mConfig.DcSimLocation;
            if (string.IsNullOrEmpty(dcSimPath))
            {
                return;
            }

            DcSimExeLauncher.Stop();
            CommServerForDcSim.Instance.ShutDown();
        }

        /// <summary>
        /// Startup DCSim with launch parameter
        /// </summary>
        public void StartUp()
        {
            var dcSimPath = mConfig.DcSimLocation;

            if (string.IsNullOrEmpty(dcSimPath))
            {
                return;
            }

            CommServerForDcSim.Instance.StartUp();

            DcSimCommandLine dcl = new DcSimCommandLine(dcSimPath)
            {
                LcSimServerPortNumber = CommServerForDcSim.Instance.ServerPortNumber
            };

            foreach (var item in mConfig.DcInstruments)//build launch parameter
            {
                dcl.AddInstrumentTypeToNextConnection((DcSimCommandLine.InstrumentKind)item.DcType);
            }

            DcSimExeLauncher.Launch(dcl);
        }
    }
}
