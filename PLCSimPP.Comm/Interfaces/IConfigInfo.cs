using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLCSimPP.Comm.Models;

namespace PLCSimPP.Comm.Interfaces
{
    public interface IConfigInfo
    {
        string SiteMapPath { get; set; }

        //int MsgReceiveInterval { get; set; }

        int MsgSendInterval { get; set; }

        string DcSimLocation { get; set; }

        ObservableCollection<AnalyzerItem> DcInstruments { get; set; }

        string DxCSimLocation { get; set; }

        ObservableCollection<AnalyzerItem> DxCInstruments { get; set; }


        string ConnectionString { get; set; }
    }
}
