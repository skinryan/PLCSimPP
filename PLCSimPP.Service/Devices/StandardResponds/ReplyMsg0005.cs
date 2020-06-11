using System.Collections.Generic;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Models;

namespace BCI.PLCSimPP.Service.Devices.StandardResponds
{
    public class ReplyMsg0005 : IResponds
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

            if (recvParam.StartsWith("11411"))
            {
                // The documentation does not explain how to fill in, this value according to the production log
                var msg10E0 = SendMsg.GetMsg10E0(unit, "0", "BUZZ CMD");
                // '04': Buzzer stop
                var buzzStop = SendMsg.GetMsg1002(unit, "1", "04", "".PadRight(15));
                // '03': Restart
                var restart = SendMsg.GetMsg1002(unit, "1", "03", "".PadRight(15));

                result.Add(msg10E0);
                result.Add(buzzStop);
                result.Add(restart);
            }

            if (recvParam.StartsWith("10010"))
            {
                // The documentation does not explain how to fill in, this value according to the production log
                var msg10e0 = SendMsg.GetMsg10E0(unit, "1", "SN10 OFF");
                // '04': Buzzer stop
                var buzzStop = SendMsg.GetMsg1002(unit, "1", "04", "".PadRight(15));

                result.Add(msg10e0);
                result.Add(buzzStop);
            }
            
            return result;
        }
    }
}