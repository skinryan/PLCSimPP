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
    public class DxCSimService : IAnalyzerSimService
    {
        private readonly IConfigService mConfigService;
        private readonly IEventAggregator mEventAggr;
        private SystemInfo mConfig;

        public bool Connected
        {
            get
            {
                return CommServerForDxCSim.Instance.HasConnectedClient;
            }
        }

        public DxCSimService(IConfigService config, IEventAggregator eventAggr)
        {
            mConfigService = config;
            mEventAggr = eventAggr;
            mConfig = mConfigService.ReadSysConfig();
            mEventAggr.GetEvent<ReLoadSiteMapEvent>().Subscribe(OnReload);
        }

        private void OnReload(bool obj)
        {
            mConfig = mConfigService.ReadSysConfig();
        }

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
