using System.Collections.Generic;
using PLCSimPP.Comm.Constants;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Comm.Models;

namespace PLCSimPP.Service.Devicies.StandardResponds
{
    internal class ReplyMsg_0004 : IResponds
    {
        public List<IMessage> GetRespondsMsg(IUnit unit, string recvParam)
        {
            List<IMessage> result = new List<IMessage>();

            IMessage cmd = new MsgCmd()
            {
                Command = UnitCmds._1001,
                Param = "A",
                UnitAddr = unit.Address,
                Port = unit.Port
            };

            result.Add(cmd);
            return result;
        }
    }
}