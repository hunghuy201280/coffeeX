using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace coffeeX.Utils
{
    class CheckValidate : ValidationRule
    {
        public string validatedPassword { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var str = value as string;
            var validate = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$");
            if (str == null)
            {
                return new ValidationResult(false, "Please enter some text");
            }
            if (!(validate.IsMatch(str) ))// TODO: set from here
            {
                return new ValidationResult(false, String.Format("Format không phù hợp", validatedPassword));
            }
            return new ValidationResult(true, null);

        }
    }
}
