using System.Collections.Generic;
using System.Collections.ObjectModel;
using PLCSimPP.Comm.Models;

namespace PLCSimPP.Comm.Interfaces
{
    public interface IRecvMsgBeheavior
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

        //void PushReceivedMsg(IMessage msg);

        /// <summary>
        /// Set the unitcollection of pipeline
        /// </summary>
        /// <param name="unitCollection">unit collection</param>
        void SetUnitCollection(ObservableCollection<IUnit> unitCollection);
    }
}