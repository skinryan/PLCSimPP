using System.Collections.Generic;
using System.Collections.ObjectModel;
using BCI.PLCSimPP.Comm.Models;

namespace BCI.PLCSimPP.Comm.Interfaces
{
    /// <summary>
    /// received message behavior interface
    /// </summary>
    public interface IRecvMsgBehavior
    {
        /// <summary>
        /// Start the receive message task
        /// </summary>
        /// <param name="token">script token</param>
        void ActiveRecvTask(string token);

        /// <summary>
        /// Stop the receive message task
        /// </summary>
        void StopRecvTask();
               
        
    }
}