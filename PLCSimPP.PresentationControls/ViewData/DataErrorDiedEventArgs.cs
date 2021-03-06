﻿using System;

namespace BCI.PLCSimPP.PresentationControls.ViewData
{
    /// <summary>
    /// DataErrorDiedEventArgs
    /// </summary>
    public class DataErrorDiedEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="source"></param>
        /// <param name="columnName"></param>
        public DataErrorDiedEventArgs(object source, string columnName)
        {
            Source = source;
            ColumnName = columnName;
        }
        /// <summary>
        /// Source
        /// </summary>
        public object Source { get; private set; }
        /// <summary>
        /// Column Name
        /// </summary>
        public string ColumnName { get; private set; }
    }
}
