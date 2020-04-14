using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using PLCSimPP.Comm;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Service.Devicies;

namespace PLCSimPP.Layout.Converter
{
    public class TypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;

            IUnit type = (IUnit)value;

            if (type.GetType() == typeof(Stocker) || type.GetType() == typeof(Outlet))
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
            //DisplayType param = (DisplayType)parameter;

            //switch (param)
            //{
            //    case DisplayType.Nomal:
            //        return GetNormalVisibility(type);
            //    case DisplayType.Storage:
            //        return GetStorageVisibility(type);
            //    default:
            //        return Visibility.Collapsed;
            //}
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
