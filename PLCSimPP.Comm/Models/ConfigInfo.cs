using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLCSimPP.Comm.Interfaces;

namespace PLCSimPP.Comm.Models
{
    public class SystemInfo : IConfigInfo
    {
        public string SiteMapPath { get; set; }
        //public int MsgReceiveInterval { get; set; }
        public int SendInterval { get; set; }
        public string DcSimLocation { get; set; }
        public ObservableCollection<AnalyzerItem> DcInstruments { get; set; }
        public string DxCSimLocation { get; set; }
        public ObservableCollection<AnalyzerItem> DxCInstruments { get; set; }

        public string SQLiteConnString { get; set; }

        public SystemInfo()
        {
            DcInstruments = new ObservableCollection<AnalyzerItem>();
            DxCInstruments = new ObservableCollection<AnalyzerItem>();
        }
    }


}
