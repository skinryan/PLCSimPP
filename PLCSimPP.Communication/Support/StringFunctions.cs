using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BCI.PLCSimPP.Communication.Support
{
    public sealed class StringFunctions
    {
        public static string CurrentTime
        {
            get
            {
                var d = DateTime.Now;
                return String.Format(CultureInfo.InvariantCulture, "{0:00}:{1:00}.{2:00}.{3:000}", d.Hour, d.Minute, d.Second, d.Millisecond);
            }
        }
    }
}
