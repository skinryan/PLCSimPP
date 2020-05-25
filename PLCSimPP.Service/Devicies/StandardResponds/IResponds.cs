using System;
using System.Collections.Generic;
using System.Text;
using BCI.PLCSimPP.Comm.Interfaces;

namespace BCI.PLCSimPP.Service.Devicies.StandardResponds
{
    public interface IResponds
    {
        List<IMessage> GetRespondsMsg(IUnit unit, string recvParam);
    }

    
}
