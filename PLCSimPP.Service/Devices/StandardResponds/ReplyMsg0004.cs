using System.Collections.Generic;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Models;

namespace BCI.PLCSimPP.Service.Devices.StandardResponds
{
    /// <inheritdoc />
    public class ReplyMsg0004 : IResponds
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

            IMessage cmd1001 = new MsgCmd()
            {
                Command = UnitCmds._1001,
                Param = "A",//'A': under AUTO Operation
                UnitAddr = unit.Address,
                Port = unit.Port
            };

            IMessage cmd1025 = new MsgCmd()
            {
                Command = UnitCmds._1025,
                Param = "1",//status '0': Stop, '1': Run
                UnitAddr = unit.Address,
                Port = unit.Port
            };

            result.Add(cmd1001);
            result.Add(cmd1025);
            return result;
        }
    }
}