using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCI.PLCSimPP.Comm.Models;

namespace BCI.PLCSimPP.Comm.Interfaces
{
    /// <summary>
    /// system settings interface
    /// </summary>
    public interface IConfigInfo
    {
        /// <summary>
        /// site map path
        /// </summary>
        string SiteMapPath { get; set; }
      
        /// <summary>
        /// Message send interval
        /// </summary>
        int SendInterval { get; set; }

        /// <summary>
        /// DCSim launch path
        /// </summary>
        string DcSimLocation { get; set; }

        /// <summary>
        /// DCSim launch parameters
        /// </summary>
        ObservableCollection<AnalyzerItem> DcInstruments { get; set; }

        /// <summary>
        /// DxCSim launch path
        /// </summary>
        string DxCSimLocation { get; set; }

        /// <summary>
        /// DxCSim launch parameters
        /// </summary>
        ObservableCollection<AnalyzerItem> DxCInstruments { get; set; }

        /// <summary>
        /// Database connection string
        /// </summary>
        string ConnectionString { get; set; }
    }
}
