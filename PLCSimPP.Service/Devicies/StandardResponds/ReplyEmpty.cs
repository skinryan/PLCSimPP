using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCI.PLCSimPP.Comm.Interfaces;

namespace BCI.PLCSimPP.Service.Devicies.StandardResponds
{
    public class ReplyEmpty : IResponds
    {
        /// <summary>
        /// This command does not require a reply
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="recvParam"></param>
        /// <returns></returns>
        public List<IMessage> GetRespondsMsg(IUnit unit, string recvParam)
        {
            return new List<IMessage>();
        }
    }
}
