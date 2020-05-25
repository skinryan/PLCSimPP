using System.Collections.Generic;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Models;

namespace BCI.PLCSimPP.Service.Devicies.StandardResponds
{
    internal class ReplyMsg_1001 : IResponds
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