using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BCI.PLCSimPP.Log.Converter
{
    public class ContentConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value != null)
            {
                var freeze = System.Convert.ToBoolean(value);

                if (freeze)
                {
                    return "Unfreeze";
                }
            }

            return "Freeze";
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //The one-way binding used here does not require a ConvertBack
            throw new NotImplementedException();
        }
    }
}
