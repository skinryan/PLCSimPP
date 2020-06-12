using System;
using System.Collections.Generic;
using System.Text;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Interfaces.Services;
using BCI.PLCSimPP.Comm.Models;

namespace BCI.PLCSimPP.Test.TestTool.Templates
{
    public class LabelerAndAliquoterTemplate : ITemplate
    {
        public void HandleMsg(IMessage msg, IRouterService mRouterService)
        {
            if (msg.UnitAddr == "0000000020")
            {
                LabelerMsg(msg, mRouterService);
            }
            else
            {
                AliquoterMsg(msg, mRouterService);
            }
        }

        private void AliquoterMsg(IMessage msg, IRouterService mRouterService)
        {
            if (msg.Command == UnitCmds._1011)
            {
                var bcr = msg.Param.Substring(0, 1);
                if (bcr == "1")
                {
                    //print order
                    var sampleId = msg.Param.Substring(1, 15);
                    MsgCmd printCmd = new MsgCmd
                    {
                        Command = LcCmds._0016,
                        Port = msg.Port,
                        UnitAddr = "0000000020",
                        Param = sampleId + "1".PadRight(15) + (sampleId + "," + "Glucose").PadRight(64)
                    };

                    var units = mRouterService.FindTargetUnit(printCmd.UnitAddr);
                    foreach (var unit in units)
                    {
                        unit.OnReceivedMsg(printCmd.Command, printCmd.Param);
                    }

                    MsgCmd reply = new MsgCmd
                    {
                        Command = LcCmds._0011,
                        Port = msg.Port,
                        UnitAddr = msg.UnitAddr,
                        Param = bcr + sampleId + "1"
                    };

                    var unitAli = mRouterService.FindTargetUnit(reply.UnitAddr);
                    foreach (var unit in unitAli)
                    {
                        unit.OnReceivedMsg(reply.Command, reply.Param);
                    }
                }

                if (bcr == "2")
                {
                    MsgCmd reply = new MsgCmd();
                    reply.Command = LcCmds._0013;
                    reply.Port = msg.Port;
                    reply.UnitAddr = msg.UnitAddr;

                    var sampleId = msg.Param.Substring(1, 15);

                    reply.Param = sampleId + "0000  ";

                    var units = mRouterService.FindTargetUnit(reply.UnitAddr);
                    foreach (var unit in units)
                    {
                        unit.OnReceivedMsg(reply.Command, reply.Param);
                    }
                }

                if (bcr == "3")
                {
                    MsgCmd reply = new MsgCmd();
                    reply.Command = LcCmds._0015;
                    reply.Port = msg.Port;
                    reply.UnitAddr = msg.UnitAddr;

                    var sampleId = msg.Param.Substring(1, 15);

                    reply.Param = sampleId + "0004FF";

                    var units = mRouterService.FindTargetUnit(reply.UnitAddr);
                    foreach (var unit in units)
                    {
                        unit.OnReceivedMsg(reply.Command, reply.Param);
                    }
                }
            }

            if (msg.Command == UnitCmds._1013)
            {
                var sampleId = msg.Param.Substring(1, 15);

                MsgCmd reply = new MsgCmd
                {
                    Command = LcCmds._0014,
                    Port = msg.Port,
                    UnitAddr = msg.UnitAddr,
                    Param = sampleId + "0004"
                };

                var units = mRouterService.FindTargetUnit(reply.UnitAddr);
                foreach (var unit in units)
                {
                    unit.OnReceivedMsg(reply.Command, reply.Param);
                }
            }
        }

        private void LabelerMsg(IMessage msg, IRouterService mRouterService)
        {
            if (msg.Command == UnitCmds._1011)
            {
                var bcr = msg.Param.Substring(0, 1);
                var sampleId = msg.Param.Substring(1, 15);

                MsgCmd reply = new MsgCmd
                {
                    Command = LcCmds._0011,
                    Port = msg.Port,
                    UnitAddr = msg.UnitAddr,
                    Param = bcr + sampleId + "1"
                };

                var units = mRouterService.FindTargetUnit(reply.UnitAddr);
                foreach (var unit in units)
                {
                    unit.OnReceivedMsg(reply.Command, reply.Param);
                }
            }
        }
    }
}
