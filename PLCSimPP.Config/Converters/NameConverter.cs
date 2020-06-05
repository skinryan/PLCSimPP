using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using BCI.PLCSimPP.Comm.Interfaces;

namespace BCI.PLCSimPP.Config.Converters
{
    public class NameConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (!(value is IUnit))
                    return string.Empty;

                var unit = (IUnit)value;
                return unit.GetType().Name + "(" + unit.DisplayName + ")";

            }
            catch (Exception)
            {
                return string.Empty;
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
