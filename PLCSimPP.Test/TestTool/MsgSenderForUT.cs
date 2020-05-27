using System;
using System.Collections.Generic;
using System.Text;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Interfaces.Services;
using BCI.PLCSimPP.Comm.Models;
using CommonServiceLocator;

namespace BCI.PLCSimPP.Test.TestTool
{
    public class MsgSenderForUT : ISendMsgBehavior
    {
        private IRouterService mRouterService;

        public string MyProperty { get; set; }

        public MsgSenderForUT()
        {
            mRouterService = ServiceLocator.Current.GetInstance<IRouterService>();
        }

        public void ActiveSendTask(string token)
        {
            //do nothing in ut
        }

        public void PushMsg(IMessage msg)
        {
            if (msg.Command == UnitCmds._1024)
            {
                MsgCmd reply = new MsgCmd();
                reply.Command = LcCmds._0012;
                reply.Port = msg.Port;
                reply.UnitAddr = msg.UnitAddr;

                var bcr = msg.Param.Substring(0, 1);
                var sampleId = msg.Param.Substring(1, 15);

                reply.Param = bcr + sampleId + "1";

                var units = mRouterService.FindTargetUnit(reply.UnitAddr);
                foreach (var unit in units)
                {
                    unit.OnReceivedMsg(reply.Command, reply.Param);
                }
            }
        }

        public void StopSendTask()
        {
            //do nothing in ut
        }
    }
}
