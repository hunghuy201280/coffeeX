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
    class TableStatusToButtonTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TableStatus status = (TableStatus)value;
            switch (status)
            {
                case TableStatus.Free:
                    return  "Thanh toán";
                case TableStatus.Pending:
                    return "Giao nước";
                case TableStatus.Done:
                    return "Làm mới";
            }
            return "Thanh toán";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
