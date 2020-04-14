﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace PLCSimPP.Config.Converters
{
    public class BooleanReverseToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (Boolean.TryParse(value.ToString(), out bool result))
                {
                    return !result;
                }
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (Boolean.TryParse(value.ToString(), out bool result))
                {
                    return !result;
                }
            }

            return true;
        }
    }
}