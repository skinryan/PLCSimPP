using System;

namespace PLCSimPP.PresentationControls.ViewData
{
    /// <summary>
    /// Interface of data error information
    /// </summary>
    public interface INotifyDataErrorInfo
    {
        /// <summary>
        /// If data has any error
        /// </summary>
        bool HaveError { get; }
        /// <summary>
        /// Error died event
        /// </summary>
        event EventHandler<DataErrorDiedEventArgs> ErrorDied;
        /// <summary>
        /// Error occured event
        /// </summary>
        event EventHandler<DataErrorOccuredEventArgs> ErrorOccured;
    }
}
