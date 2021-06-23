﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace coffeeX.Utils
{
    class DoubleToVNDConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double valueToConvert = (double)value;
            string res= String.Format("{0:n0}", valueToConvert) +" Đ";
            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            /*string val = value as string;
            val*/
            return 1;
        }
    }
}