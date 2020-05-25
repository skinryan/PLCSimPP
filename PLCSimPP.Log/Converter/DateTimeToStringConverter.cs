using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace BCI.PLCSimPP.Log.Converter
{
    public class DateTimeToStringConverter : IValueConverter
    {
        const string FORMAT_PATTERN = "yyyy-MM-dd HH:mm:ss.fff";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                DateTime dt = (DateTime)value;

                return dt.ToString(FORMAT_PATTERN);
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
