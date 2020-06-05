using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BCI.PLCSimPP.Log.Converter
{
    public class BoolReverseConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null)
                {
                    var flag = System.Convert.ToBoolean(value);
                    return !flag;
                }

                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //The one-way binding used here does not require a ConvertBack
            throw new NotImplementedException();
        }
    }
}
