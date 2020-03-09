using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using PLCSimPP.Comm.Interfaces.Services;
using PLCSimPP.Comm.Models;

namespace PLCSimPP.Comm.Interfaces
{
    public interface IPipeLine
    {
        /// <summary>
        /// Connect to Line controller flag
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Unit collection
        /// </summary>
        ObservableCollection<UnitBase> UnitCollection { get; set; }

        /// <summary>
        /// Analyzer simulator 
        /// </summary>
        IAnalyzerSimBehavior AnalyzerSim { get; set; }

        /// <summary>
        /// msg receive/send handler
        /// </summary>
        IMsgService MsgService { get; set; }

        /// <summary>
        /// Load sample on line
        /// </summary>
        /// <param name="sample">sample tube</param>
        void LoadSample(Sample sample);

        /// <summary>
        /// Reset all pipeline unit status
        /// </summary>
        /// <returns></returns>
        bool Init();

        /// <summary>
        /// Connect LC
        /// </summary>
        /// <returns></returns>
        bool Connect();

        /// <summary>
        /// Disconnect LC
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Rack exchange 
        /// </summary>
        /// <param name="address">address</param>
        /// <param name="shelf">floor</param>
        /// <param name="rack">rack</param>
        void RackExchange(string address, string shelf, string rack);
    }
}
