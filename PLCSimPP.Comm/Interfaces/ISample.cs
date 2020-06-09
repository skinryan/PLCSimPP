using BCI.PLCSimPP.Comm.Enums;

namespace BCI.PLCSimPP.Comm.Interfaces
{
    public interface ISample
    {
        /// <summary>
        /// Sample ID
        /// </summary>
        string SampleID { get; set; }

        RackType Rack { get; set; }

        bool IsLoaded { get; set; }

        string DcToken { get; set; }

        string DxCToken { get; set; }

        bool IsSubTube { get; set; }

        bool Retrieving { get; set; }
    }
}