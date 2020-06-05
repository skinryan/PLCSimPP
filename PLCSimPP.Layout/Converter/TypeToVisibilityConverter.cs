using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using BCI.PLCSimPP.Comm;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Service.Devicies;

namespace BCI.PLCSimPP.Layout.Converter
{
    public class TypeToVisibilityConverter : IValueConverter
    {
        /// <inheritdoc/>
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
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //The one-way binding used here does not require a ConvertBack
            throw new NotImplementedException();
        }
    }
}
