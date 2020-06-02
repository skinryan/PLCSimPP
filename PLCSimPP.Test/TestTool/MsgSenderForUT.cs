using System;
using System.Collections.Generic;
using System.Text;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Interfaces.Services;
using BCI.PLCSimPP.Comm.Models;
using BCI.PLCSimPP.Test.TestTool.Templates;
using CommonServiceLocator;

namespace BCI.PLCSimPP.Test.TestTool
{
    public class MsgSenderForUT : ISendMsgBehavior
    {
        private IRouterService mRouterService;

        public List<IMessage> MessageList = new List<IMessage>();

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
            MessageList.Add(msg);
            var temp = MsgTemplate.GetTemplate(msg.UnitAddr);

            temp.HandleMsg(msg, mRouterService);
        }

        private void Reply0011(IMessage msg)
        {
           
        }

        public void StopSendTask()
        {
            //do nothing in ut
        }
    }





   
}
