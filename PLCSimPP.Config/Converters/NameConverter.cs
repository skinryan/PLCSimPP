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
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value is IUnit)
                {
                    IUnit unit = (IUnit)value;
                    return unit.GetType().Name + "(" + unit.DisplayName + ")";
                }

                return string.Empty;
            }
            catch (Exception)
            {

                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
