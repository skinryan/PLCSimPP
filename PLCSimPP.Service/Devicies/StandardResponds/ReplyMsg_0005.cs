using System.Collections.Generic;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Models;

namespace BCI.PLCSimPP.Service.Devicies.StandardResponds
{
    public class ReplyMsg_0005 : IResponds
    {
        /// <summary>
        /// build responds msg
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="recvParam"></param>
        /// <returns></returns>
        public List<IMessage> GetRespondsMsg(IUnit unit, string recvParam)
        {
            List<IMessage> result = new List<IMessage>();

            // The documentation does not explain how to fill in, this value according to the production log
            var msg10e0 = SendMsg.GetMsg_10E0(unit, "0", "BUZZ CMD");
            // '04': Buzzer stop
            var buzzStop = SendMsg.GetMsg_1002(unit, "1", "04", "".PadRight(15));
            // '03': Restart
            var restart = SendMsg.GetMsg_1002(unit, "1", "03", "".PadRight(15));

            result.Add(msg10e0);
            result.Add(buzzStop);
            result.Add(restart);
            return result;
        }
    }
}