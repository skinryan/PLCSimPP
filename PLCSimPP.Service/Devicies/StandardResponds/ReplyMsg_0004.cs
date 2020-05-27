using System.Collections.Generic;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Models;

namespace BCI.PLCSimPP.Service.Devicies.StandardResponds
{
    public class ReplyMsg_0004 : IResponds
    {
        /// <summary>
        /// Get Responds Msg 
        /// </summary>
        /// <param name="unit">the unit responsible for processing the message</param>
        /// <param name="recvParam">command param</param>
        /// <returns></returns>
        public List<IMessage> GetRespondsMsg(IUnit unit, string recvParam)
        {
            List<IMessage> result = new List<IMessage>();

            IMessage cmd = new MsgCmd()
            {
                Command = UnitCmds._1001,
                Param = "A",//'A': under AUTO Operation
                UnitAddr = unit.Address,
                Port = unit.Port
            };

            result.Add(cmd);
            return result;
        }
    }
}