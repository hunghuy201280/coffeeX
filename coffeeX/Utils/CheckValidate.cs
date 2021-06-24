using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace coffeeX.Utils
{
    class CheckValidate : ValidationRule
    {

       
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            
            var str = value as string;
            var validate = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$");
            if (str == null)
            {
             
                return new ValidationResult(false, "Vui lòng nhập mật khẩu");
            }
            if (!(validate.IsMatch(str) ))
            {
         
                return new ValidationResult(false, String.Format("Format không phù hợp"));
            }
   
            return new ValidationResult(true, null);

        }
    }
}
