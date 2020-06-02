using System;
using System.Collections.Generic;
using System.Text;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Interfaces.Services;

namespace BCI.PLCSimPP.Test.TestTool.Templates
{
    public class OutletTemplate : ITemplate
    {
        public void HandleMsg(IMessage msg, IRouterService mRouterService)
        {
            throw new NotImplementedException();
        }
    }
}
