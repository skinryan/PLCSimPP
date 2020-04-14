using System.Collections.Generic;
using PLCSimPP.Comm.Constants;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Comm.Models;

namespace PLCSimPP.Service.Devicies.StandardResponds
{
    public class ReplyMsg_0005 : IResponds
    {
        public List<IMessage> GetRespondsMsg(IUnit unit, string recvParam)
        {
            List<IMessage> result = new List<IMessage>();

            IMessage cmd = new MsgCmd()
            {
                Command = UnitCmds._10E0,
                Param = "0BUZZ CMD",
                UnitAddr = unit.Address,
                Port = unit.Port
            };

            result.Add(cmd);
            return result;
        }
    }
}