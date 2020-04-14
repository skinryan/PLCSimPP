using System;

namespace PLCSimPP.PresentationCommon.ViewData
{
    /// <summary>
    /// DataErrorOccuredEventArgs
    /// </summary>
    public class DataErrorOccuredEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataErrorInformation"></param>
        public DataErrorOccuredEventArgs(DataErrorInformation dataErrorInformation)
        {
            DataErrorInformation = dataErrorInformation;
        }
        /// <summary>
        /// Data Error Information
        /// </summary>
        public DataErrorInformation DataErrorInformation { get; private set; }
    }
}
