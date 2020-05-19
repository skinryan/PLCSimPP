using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DcSimCom;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Comm.Interfaces.Services;
using PLCSimPP.Comm.Models;

namespace PLCSimPP.Service.Analyzer
{
    public class DCSimService : IAnalyzerSimService
    {
        private readonly IConfigService mConfigService;
        private SystemInfo mConfig;

        public DCSimService(IConfigService config)
        {
            mConfigService = config;
            mConfig = mConfigService.ReadSysConfig();
        }

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

        public void ShutDown()
        {
            DcSimExeLauncher.Stop();
            CommServerForDcSim.Instance.ShutDown();
        }

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

            foreach (var item in mConfig.DcInstruments)
            {
                dcl.AddInstrumentTypeToNextConnection((DcSimCommandLine.InstrumentKind)item.DcType);
            }

            DcSimExeLauncher.Launch(dcl);
        }
    }
}
