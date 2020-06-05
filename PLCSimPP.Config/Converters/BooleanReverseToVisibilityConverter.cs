using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace BCI.PLCSimPP.Config.Converters
{
    public class BooleanReverseToVisibilityConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            if (bool.TryParse(value.ToString(), out var result))
            {
                return !result;
            }

            return false;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return true;

            if (bool.TryParse(value.ToString(), out var result))
            {
                return !result;
            }

            return true;
        }
    }
}
