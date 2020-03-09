using System.Collections.Generic;
using PLCSimPP.Comm.Models;

namespace PLCSimPP.Comm.Interfaces
{
    public interface IRecvMsgBeheavior
    {
        /// <summary>
        /// Pause processing message flag
        /// </summary>
        bool IsPaused { get; set; }
               
        /// <summary>
        /// Start the receive message task
        /// </summary>
        /// <param name="token">script token</param>
        void ActiveRecvTask(string token);

        /// <summary>
        /// Stop the receive message task
        /// </summary>
        void StopRecvTask();

        /// <summary>
        /// Set the unitcollection of pipeline
        /// </summary>
        /// <param name="unitCollection">unit collection</param>
        void SetUnitCollection(IEnumerable<UnitBase> unitCollection);
    }
}