using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using BCI.PLCSimPP.Comm;
using BCI.PLCSimPP.Comm.Enums;
using BCI.PLCSimPP.Comm.Helper;

namespace BCI.PLCSimPP.Layout.Converter
{
    public class RackToStrConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                RackType rt = (RackType)value;

                return EnumHelper.GetEnumDescription(rt);
            }

            return string.Empty;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //The one-way binding used here does not require a ConvertBack
            throw new NotImplementedException();
        }
    }
}
