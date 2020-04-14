using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DcSimCom;
using DxCSimCom;
using DxCSimCom.ToDxCSimMessage;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Comm.Interfaces.Services;
using PLCSimPP.Comm.Models;

namespace PLCSimPP.Service.Analyzer
{
    public class DxCSimService : IAnalyzerSimBehavior
    {
        private readonly IConfigService mConfigService;
        private SystemInfo mConfig;

        public DxCSimService(IConfigService config)
        {
            mConfigService = config;
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
                LcSimServerPortNumber = CommServerForDcSim.Instance.ServerPortNumber
            };

            foreach (var item in mConfig.DxCInstruments)
            {
                dxcl.AddInstrumentTypeToNextConnection((DxCSimCommandLine.SynchronInstrumentKind)item.DxCType);
            }

            DxCSimExeLauncher.Launch(dxcl);
        }
    }
}
