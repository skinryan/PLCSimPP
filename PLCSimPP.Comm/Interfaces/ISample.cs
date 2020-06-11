using BCI.PLCSimPP.Comm.Enums;

namespace BCI.PLCSimPP.Comm.Interfaces
{
    /// <summary>
    /// sample interface
    /// </summary>
    public interface ISample
    {
        /// <summary>
        /// Sample ID
        /// </summary>
        string SampleID { get; set; }

        /// <summary>
        /// rack type
        /// </summary>
        RackType Rack { get; set; }

        /// <summary>
        /// mark the sample loaded to automation system
        /// </summary>
        bool IsLoaded { get; set; }

        /// <summary>
        /// the token will send to DCSim when sample loaded to GC connection
        /// </summary>
        string DcToken { get; set; }

        /// <summary>
        /// the token will send to DCSim when sample loaded to DxC connection
        /// </summary>
        string DxCToken { get; set; }

        /// <summary>
        /// mark the tube is secondary tube
        /// </summary>
        bool IsSubTube { get; set; }

        /// <summary>
        /// mark the tube is retrieving
        /// </summary>
        bool Retrieving { get; set; }
    }
}