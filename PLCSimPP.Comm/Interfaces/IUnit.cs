using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml.Serialization;

namespace BCI.PLCSimPP.Comm.Interfaces
{
    public interface IUnit
    {
        bool IsMaster { get; set; }
        /// <summary>
        /// Port
        /// </summary>
        int Port { get; set; }

        /// <summary>
        /// Pending sample count
        /// </summary>
        int PendingCount { get; }

        /// <summary>
        /// Unit address
        /// </summary>
        string Address { get; set; }

        /// <summary>
        /// displayName
        /// </summary>
        string DisplayName { get; set; }

        ///// <summary>
        ///// Unit type
        ///// </summary>
        //UnitType UnitType { get; }

        /// <summary>
        /// Children unit collection
        /// </summary>
        ObservableCollection<IUnit> Children { get; }

        /// <summary>
        /// If contain child unit
        /// </summary>
        bool HasChild { get; }

        /// <summary>
        /// Parent unit (null if there are no parent)
        /// </summary>
        IUnit Parent { get; set; }

        /// <summary>
        /// the sample in processing
        /// </summary>
        ISample CurrentSample { get; }

        /// <summary>
        /// Put the sample on the processing queue
        /// </summary>
        /// <param name="sample"></param>
        void EnqueueSample(ISample sample);

        /// <summary>
        /// clear jammed sample
        /// </summary>
        void ResetQueue();

        /// <summary>
        /// Processing when a message is received
        /// </summary>
        /// <param name="cmd">command code</param>
        /// <param name="content">command content</param>
        void OnReceivedMsg(string cmd, string content);

        /// <summary>
        /// InitUnit
        /// </summary>
        void InitUnit();

    }
}
