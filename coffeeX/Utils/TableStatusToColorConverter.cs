using coffeeX.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace coffeeX.Utils
{
    class TableStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TableStatus status = (TableStatus)value;
            switch(status)
            {
                case TableStatus.Free:
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDADADA"));
                case TableStatus.Pending:
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFEA8025"));
                case TableStatus.Done:
                    return new SolidColorBrush(Colors.Green);
                

            }
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDADADA"));

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
