using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSimPP.PresentationControls.ViewData
{
    /// <summary>
    /// Data validation error information
    /// </summary>
    public class DataErrorInformation
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="source"></param>
        /// <param name="columnName"></param>
        /// <param name="errorMessage"></param>
        public DataErrorInformation(object source, string columnName, string errorMessage)
        {
            Source = source;
            ColumnName = columnName;
            ErrorMessage = errorMessage;
        }
        /// <summary>
        /// Source
        /// </summary>
        public object Source { get; private set; }
        /// <summary>
        /// Column Name
        /// </summary>
        public string ColumnName { get; private set; }
        /// <summary>
        /// Error Message
        /// </summary>
        public string ErrorMessage { get; private set; }
    }
}
