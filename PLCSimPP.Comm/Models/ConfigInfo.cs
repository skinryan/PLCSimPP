using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCI.PLCSimPP.Comm.Interfaces;

namespace BCI.PLCSimPP.Comm.Models
{
    /// <summary>
    /// The implementation of IConfigInfo
    /// </summary>
    public class SystemInfo : IConfigInfo
    {
        /// <inheritdoc />
        public string SiteMapPath { get; set; }
        /// <inheritdoc />
        public int SendInterval { get; set; }
        /// <inheritdoc />
        public string DcSimLocation { get; set; }
        /// <inheritdoc />
        public ObservableCollection<AnalyzerItem> DcInstruments { get; set; }
        /// <inheritdoc />
        public string DxCSimLocation { get; set; }
        /// <inheritdoc />
        public ObservableCollection<AnalyzerItem> DxCInstruments { get; set; }
        /// <inheritdoc />
        public string ConnectionString { get; set; }
        /// <summary>
        /// constructor
        /// </summary>
        public SystemInfo()
        {
            DcInstruments = new ObservableCollection<AnalyzerItem>();
            DxCInstruments = new ObservableCollection<AnalyzerItem>();
        }
    }


}
