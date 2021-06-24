using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace coffeeX.Utils
{
    class PriceValidationRule : ValidationRule
    {

        public string MustEndWith { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var str = value as string;
            if (str == null)
            {
                return new ValidationResult(false, "Vui lòng nhập giá");
            }
            if (!str.EndsWith(MustEndWith))
            {
                return new ValidationResult(false, String.Format("Sai định dạng '{0}'", MustEndWith));
            }
            return new ValidationResult(true, null);

        }

    }
}
