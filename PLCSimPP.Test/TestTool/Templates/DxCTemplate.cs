using System;
using System.Collections.Generic;
using System.Text;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Interfaces.Services;
using BCI.PLCSimPP.Comm.Models;

namespace BCI.PLCSimPP.Test.TestTool.Templates
{
    public class DxCTemplate : ITemplate
    {
        public void HandleMsg(IMessage msg, IRouterService mRouterService)
        {
            if (msg.Command == UnitCmds._1011)
            {
                var bcr = msg.Param.Substring(0, 1);

                MsgCmd reply = new MsgCmd();
                reply.Command = LcCmds._0011;
                reply.Port = msg.Port;
                reply.UnitAddr = msg.UnitAddr;
                var sampleId = msg.Param.Substring(1, 15);
                reply.Param = bcr + sampleId + "1";

                var units = mRouterService.FindTargetUnit(reply.UnitAddr);
                foreach (var unit in units)
                {
                    unit.OnReceivedMsg(reply.Command, reply.Param);
                }

            }
        }
    }
}
