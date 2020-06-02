using System;
using System.Collections.Generic;
using System.Text;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Interfaces.Services;
using BCI.PLCSimPP.Test.ServiceTest;

namespace BCI.PLCSimPP.Test.TestTool.Templates
{
    public interface ITemplate
    {
        void HandleMsg(IMessage msg, IRouterService mRouterService);
    }

    public class MsgTemplate
    {
        //unitAddress base on the TestDataSource.GetLayout() method result
        public static ITemplate GetTemplate(string unitAddress)
        {
            switch (unitAddress)
            {
                case "0000000002":
                    return new InletTemplate();
                case "0000000001":
                    return new HMOutletTemplate();
                case "0000000004":
                    return new CentrifugeTemplate();
                case "0000000010":
                    return new LevelDetectorTemplate();
                case "0000000020":                    
                case "0000000040":
                    return new LabelerAndAliquoterTemplate();
                case "0000000100":
                    return new GCTemplate();
                case "0000200000":
                    return new DxCTemplate();
                case "0002000000":
                    return new StockerTemplate();
                case "0010000000":
                    return new OutletTemplate();
                default:
                    return new GeneralTemplate();
            }
        }
    }


}
