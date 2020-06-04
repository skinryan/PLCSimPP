using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Interfaces.Services;
using BCI.PLCSimPP.Comm.Models;

namespace BCI.PLCSimPP.Test.TestTool.Templates
{
    public class StockerTemplate : ITemplate
    {
        public void HandleMsg(IMessage msg, IRouterService mRouterService)
        {
            if (msg.Command == UnitCmds._1011)
            {
                var bcr = msg.Param.Substring(0, 1);
                var sampleId = msg.Param.Substring(1, 15);

                if (bcr == "1")
                {
                    if (sampleId.Contains("0001"))
                    {
                        MsgCmd reply = new MsgCmd();
                        reply.Command = LcCmds._0012;
                        reply.Port = msg.Port;
                        reply.UnitAddr = msg.UnitAddr;
                        reply.Param = bcr + sampleId;

                        var units = mRouterService.FindTargetUnit(reply.UnitAddr);
                        foreach (var unit in units)
                        {
                            unit.OnReceivedMsg(reply.Command, reply.Param);
                        }
                    }
                    else
                    {
                        MsgCmd reply = new MsgCmd();
                        reply.Command = LcCmds._0017;
                        reply.Port = msg.Port;
                        reply.UnitAddr = msg.UnitAddr;
                        reply.Param = bcr + sampleId + "11007K31";

                        var units = mRouterService.FindTargetUnit(reply.UnitAddr);
                        foreach (var unit in units)
                        {
                            unit.OnReceivedMsg(reply.Command, reply.Param);
                        }

                        Thread.Sleep(5000);

                        MsgCmd retrive = new MsgCmd();
                        retrive.Command = LcCmds._0019;
                        retrive.Port = msg.Port;
                        retrive.UnitAddr = msg.UnitAddr;
                        retrive.Param = sampleId + "11007";

                        foreach (var unit in units)
                        {
                            unit.OnReceivedMsg(retrive.Command, retrive.Param);
                        }
                    }
                }
                
                if (bcr == "2")
                {
                    MsgCmd reply = new MsgCmd();
                    reply.Command = LcCmds._0011;
                    reply.Port = msg.Port;
                    reply.UnitAddr = msg.UnitAddr;
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
}
