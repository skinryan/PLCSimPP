using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace BCI.PLCSimPP.Log.CustomControl
{
    public class DatePageRoutedEventArgs : RoutedEventArgs
    {
        public DatePageRoutedEventArgs(RoutedEvent routed, object source) :
            base(routed, source)
        { }

        /// <summary>
        /// Current page index
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Total page count
        /// </summary>
        public int PageCount { get; set; }
    }
}
