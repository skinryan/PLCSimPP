using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using PLCSimPP.Comm;
using PLCSimPP.Comm.Helper;

namespace PLCSimPP.Layout.Converter
{
    public class RackToStrConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                RackType rt = (RackType)value;

                return EnumHelper.GetEnumDescription(rt);
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
