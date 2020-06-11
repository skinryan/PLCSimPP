using System;
using System.Collections.Generic;
using System.Text;
using BCI.PLCSimPP.Comm.Interfaces;

namespace BCI.PLCSimPP.Service.Devices.StandardResponds
{
    /// <summary>
    /// common responds interface
    /// </summary>
    public interface IResponds
    {
        /// <summary>
        /// get common sequence responds
        /// </summary>
        /// <param name="unit">unit</param>
        /// <param name="recvParam">received message param</param>
        /// <returns></returns>
        List<IMessage> GetRespondsMsg(IUnit unit, string recvParam);
    }

    
}
