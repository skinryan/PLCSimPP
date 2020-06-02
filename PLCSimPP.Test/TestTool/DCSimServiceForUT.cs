using System;
using System.Collections.Generic;
using System.Text;
using BCI.PLCSimPP.Comm.Interfaces;

namespace BCI.PLCSimPP.Test.TestTool
{
    public class AnalyzerSimServiceForUT : IAnalyzerSimService
    {
        public void SendMsg(int unitNum, string token, string sampleId)
        {
            //DO nothing for ut
        }

        public void ShutDown()
        {
            //DO nothing for ut
        }

        public void StartUp()
        {
            //DO nothing for ut
        }
    }
}
