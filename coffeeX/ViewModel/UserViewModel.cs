using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffeeX.ViewModel
{

    class UserViewModel : BaseViewModel
    {
        private string _userName;
        private string _password;
        UserViewModel()
        {

        }

        public string UserName { get => _userName; set { _userName = value; OnPropertyChanged(); } }

        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }
    }
}
