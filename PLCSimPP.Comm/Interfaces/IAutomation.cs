using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using BCI.PLCSimPP.Comm.Interfaces.Services;
using BCI.PLCSimPP.Comm.Models;

namespace BCI.PLCSimPP.Comm.Interfaces
{
    public interface IAutomation
    {
        int OnlineSampleCount { get; }

        /// <summary>
        /// Connect to Line controller flag
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Unit collection
        /// </summary>
        ObservableCollection<IUnit> UnitCollection { get; set; }

        /// <summary>
        /// msg receive/send handler
        /// </summary>
        IPortService PortService { get; set; }

        /// <summary>
        /// Load sample on line
        /// </summary>
        /// <param name="sample">sample tube</param>
        void LoadSample(List<ISample> samples);

        /// <summary>
        /// Reset all pipeline unit status
        /// </summary>
        /// <returns></returns>
        void Init();

        /// <summary>
        /// Connect LC
        /// </summary>
        /// <returns></returns>
        void Connect();

        /// <summary>
        /// Disconnect LC
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Rack exchange 
        /// </summary>
        /// <param name="unit">unit</param>
        /// <param name="shelf">floor</param>
        /// <param name="rack">rack</param>
        void RackExchange(IUnit unit, string shelf, string rack);
    }
}
