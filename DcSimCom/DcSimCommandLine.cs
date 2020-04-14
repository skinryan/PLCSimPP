using System.Collections.Generic;
using System.Text;

namespace DcSimCom
{
    /// <summary>
    /// Describes the command line used to invoke DcSim.
    /// 
    /// Command line parameters: [Parameter 1], [Parameter2], ..., [Parameter n]
    ///   Parameter 1: Number of connections.
    ///   Parameter 2: LcSim server port number.
    ///   Parameter 3: Instrument type at connection 1 (DCInstrument = 1, AUInstrument = 2)
    ///   Parameter n: Instrument type at connection (n-2)
    /// </summary>
    public class DcSimCommandLine
    {
        /// <summary>
        /// The int values must match what is in DCSim (modDCSim.bas).
        /// </summary>
        public enum InstrumentKind
        {
            /// <summary>
            /// Default instrument type
            /// </summary>
            NondynamicInstrument = 0,
            /// <summary>
            /// DXI instrument Type
            /// </summary>
            IdcInstrument = 1,
            /// <summary>
            /// AU instrument type
            /// </summary>
            AuInstrument = 2,

            /// <summary>
            /// DXH instrument type
            /// </summary>
            DxHInstrument = 3
        }

        /// <summary>
        /// Executable DCSim file path
        /// </summary>
        public string DcSimExePath { get; private set; }

        private readonly List<InstrumentKind> mInstrumentTypes = new List<InstrumentKind>();

        /// <summary>
        /// Number of configured Dynamic instruments
        /// </summary>
        public int NumberInstrumentTypes
        {
            get
            {
                return mInstrumentTypes.Count;
            }
        }

        /// <summary>
        /// TCP/IP port to communicate with LCSIM
        /// </summary>
        public int LcSimServerPortNumber { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dcSimExePathArg"></param>
        public DcSimCommandLine(string dcSimExePathArg)
        {
            DcSimExePath = dcSimExePathArg;
        }

        /// <summary>
        /// Add configured dynamic connection to the List
        /// </summary>
        /// <param name="instrumentTypeArg"></param>
        public void AddInstrumentTypeToNextConnection(InstrumentKind instrumentTypeArg)
        {
            mInstrumentTypes.Add(instrumentTypeArg);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            var result = new StringBuilder();
            result.AppendFormat("{0},{1}", mInstrumentTypes.Count, LcSimServerPortNumber);
            mInstrumentTypes.ForEach(instrumentType => result.AppendFormat(",{0}", (int)instrumentType));

            return result.ToString();
        }
    }
}