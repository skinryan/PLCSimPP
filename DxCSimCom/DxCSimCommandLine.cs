using System.Collections.Generic;
using System.Text;

namespace DxCSimCom
{
    /// <summary>
    /// Describes the command line used to invoke DxCSim.
    /// 
    /// Command line parameters: [Parameter 1], [Parameter2], ..., [Parameter n]
    ///   Parameter 1: Number of connections.
    ///   Parameter 2: LcSim server port number.
    ///   Parameter 3: Instrument type at connection 1 (DCInstrument = 1, AUInstrument = 2)
    ///   Parameter n: Instrument type at connection (n-2)
    /// </summary>
    public class DxCSimCommandLine
    {
        /// <summary>
        /// The int values must match what is in DxCSim (modLXSim.bas).
        /// </summary>
        public enum SynchronInstrumentKind
        {
            /// <summary>
            /// Default
            /// </summary>
            None = 0,
            /// <summary>
            /// THE LX20 id
            /// Must match with the Instrument type id in DXCSim
            /// Note: This is currently not be used
            /// </summary>
            LX20Instrument = 1,
            /// <summary>
            /// The DxC id
            /// Must match with the Instrument type id in DXCSim
            /// </summary>
            DxCInstrument = 2,
        }

        /// <summary>
        /// Executable DxCsim file path
        /// </summary>
        public string DxCSimExePath { get; private set; }

        private readonly List<SynchronInstrumentKind> mInstruments = new List<SynchronInstrumentKind>();

        /// <summary>
        /// The number of configured DXC 
        /// </summary>
        public int NumberOfInstruments
        {
            get { return mInstruments.Count; }
        }

        /// <summary>
        /// TCP/IP Port communicate with LCSim
        /// </summary>
        public int LcSimServerPortNumber { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dxCSimExePath"></param>
        public DxCSimCommandLine(string dxCSimExePath)
        {
            DxCSimExePath = dxCSimExePath;
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
            result.AppendFormat("{0},{1}", mInstruments.Count, LcSimServerPortNumber);
            mInstruments.ForEach(instrumentType => result.AppendFormat(",{0}", (int)instrumentType));
            return result.ToString();
        }

        /// <summary>
        /// Add DxC type instrument to the List
        /// </summary>
        /// <param name="instrumentType"></param>
        public void AddInstrumentTypeToNextConnection(SynchronInstrumentKind instrumentType)
        {
            mInstruments.Add(instrumentType);
        }
    }
}
