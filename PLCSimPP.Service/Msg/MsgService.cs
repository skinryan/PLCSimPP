using System;
using System.Collections.Generic;
using System.Text;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Comm.Interfaces.Services;

namespace PLCSimPP.Service.Services
{
    public class MsgService : IMsgService
    {
        public PortConnection MasterPort_1 { get; set; }
        public PortConnection MasterPort_2 { get; set; }
        public PortConnection MasterPort_3 { get; set; }

        public IRecvMsgBeheavior MsgReceivedHandler
        {
            get; set;
        }

        public ISendMsgBehavior MsgSenderHandler
        {
            get; set;
        }

        public void ConnectAll()
        {
            throw new System.NotImplementedException();
        }

        private void MsgReceived()
        {
            throw new System.NotImplementedException();
        }

        public MsgService()
        {
            //TODO: read config build port conections



        }
    }
}
