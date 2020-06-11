using System.Collections.Generic;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Models;

namespace BCI.PLCSimPP.Service.Devices.StandardResponds
{
    /// <inheritdoc />
    public class ReplyMsg0006 : IResponds
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