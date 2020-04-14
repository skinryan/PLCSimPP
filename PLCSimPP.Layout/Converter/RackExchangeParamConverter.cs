using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PLCSimPP.Layout.Converter
{
    public class RackExchangeParamConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            List<object> result = new List<object>();
            foreach (var obj in values)
            {
                if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
                {
                    result.Add(obj);
                }
            }
            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
