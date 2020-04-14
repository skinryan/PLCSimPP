using System;
using System.Collections.Generic;
using System.Text;
using PLCSimPP.Comm.Interfaces;

namespace PLCSimPP.Service.Devicies.StandardResponds
{
    public interface IResponds
    {
        List<IMessage> GetRespondsMsg(IUnit unit, string recvParam);
    }

    
}
