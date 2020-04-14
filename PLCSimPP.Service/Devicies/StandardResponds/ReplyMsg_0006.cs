using System.Collections.Generic;
using PLCSimPP.Comm.Constants;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Comm.Models;

namespace PLCSimPP.Service.Devicies.StandardResponds
{
    public class ReplyMsg_0006 : IResponds
    {
        public List<IMessage> GetRespondsMsg(IUnit unit, string recvParam)
        {
            //REPLY arrived 
            List<IMessage> result = new List<IMessage>();

            try
            {
                var bcrNo = recvParam.Trim();

                IMessage cmd = new MsgCmd()
                {
                    Command = UnitCmds._1011,
                    Param = bcrNo + unit.CurrentSample.SampleID.PadRight(15),
                    UnitAddr = unit.Address,
                    Port = unit.Port
                };

                result.Add(cmd);
                return result;
            }
            catch (System.Exception)
            {
                return result;
            }

        }
    }
}