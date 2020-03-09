using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Comm.Interfaces.Services;
using PLCSimPP.Comm.Models;

namespace PLCSimPP.Service.Router
{
    public class PipeLineService : IPipeLine
    {

        public bool IsConnected => throw new NotImplementedException();

        public ObservableCollection<UnitBase> UnitCollection { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IAnalyzerSimBehavior AnalyzerSim { get; set; }
        public IMsgService MsgService { get; set; }

        public IRouterService RouterService { get; set; }

        public IConfigService ConfigService { get; set; }

        public bool Connect()
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public void LoadSample(Sample sample)
        {
            throw new NotImplementedException();
        }

        public void RackExchange(string address, string shelf, string rack)
        {
            throw new NotImplementedException();
        }

        public bool Init()
        {
            throw new NotImplementedException();
        }
    }
}
