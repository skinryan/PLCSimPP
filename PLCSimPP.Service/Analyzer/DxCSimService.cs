using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DcSimCom;
using DxCSimCom;
using DxCSimCom.ToDxCSimMessage;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Interfaces.Services;
using BCI.PLCSimPP.Comm.Models;
using Prism.Events;
using BCI.PLCSimPP.Comm.Events;

namespace BCI.PLCSimPP.Service.Analyzer
{
    /// <summary>
    /// DxCSimService
    /// </summary>
    public class DxCSimService : IAnalyzerSimService
    {
        private readonly IConfigService mConfigService;
        private readonly IEventAggregator mEventAggr;
        private SystemInfo mConfig;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="config"></param>
        /// <param name="eventAggr"></param>
        public DxCSimService(IConfigService config, IEventAggregator eventAggr)
        {
            mConfigService = config;
            mEventAggr = eventAggr;
            mConfig = mConfigService.ReadSysConfig();
            mEventAggr.GetEvent<ReLoadSiteMapEvent>().Subscribe(OnReload);
        }

        /// <summary>
        /// reload config
        /// </summary>
        /// <param name="obj"></param>
        private void OnReload(bool obj)
        {
            mConfig = mConfigService.ReadSysConfig();
        }

        /// <summary>
        /// send message to DxCSim
        /// </summary>
        /// <param name="unitNum"></param>
        /// <param name="token"></param>
        /// <param name="sampleId"></param>
        public void SendMsg(int unitNum, string token, string sampleId)
        {
            LoadSampleOnDxCMessage analyzerMsg = new LoadSampleOnDxCMessage()
            {
                DxCInstrTokenId = token,
                SampleId = sampleId,
                UnitNumber = unitNum
            };
            CommServerForDxCSim.Instance.Send(analyzerMsg);
        }

        /// <summary>
        /// Shut down DxCSim
        /// </summary>
        public void ShutDown()
        {
            var dxcSimPath = mConfig.DxCSimLocation;
            if (string.IsNullOrEmpty(dxcSimPath))
            {
                return;
            }
            DxCSimExeLauncher.Stop();
            CommServerForDxCSim.Instance.ShutDown();
        }

        /// <summary>
        /// Startup DxCSim with launch parameter
        /// </summary>
        public void StartUp()
        {
            var dxcSimPath = mConfig.DxCSimLocation;

            if (string.IsNullOrEmpty(dxcSimPath))
            {
                return;
            }

            CommServerForDxCSim.Instance.StartUp();

            DxCSimCommandLine dxcl = new DxCSimCommandLine(dxcSimPath)
            {
                LcSimServerPortNumber = CommServerForDxCSim.Instance.ServerPortNumber
            };

            foreach (var item in mConfig.DxCInstruments)
            {
                dxcl.AddInstrumentTypeToNextConnection((DxCSimCommandLine.SynchronInstrumentKind)item.DxCType);
            }

            DxCSimExeLauncher.Launch(dxcl);
        }
    }
}
