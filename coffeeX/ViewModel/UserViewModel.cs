using coffeeX.Repository;
using coffeeX.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace coffeeX.ViewModel
{

    public class UserViewModel : BaseViewModel
    {
        public ICommand registerCommand { get; set; }

        public bool isLogin { get; set; }

        private string _userName;
        public string userName { get => _userName; set { _userName = value; OnPropertyChanged(); } }


        private string _password;
        public string password { get => _password; set { _password = value; OnPropertyChanged(); } }


        private string _fullName;

        public string fullName //<-------- dung cai nay check empty
        {
            get => _fullName; set
            {
                _fullName = value;
                OnPropertyChanged();
            }
        }


        private string _phoneNumber;

        public string phoneNumber
        {
            get => _phoneNumber; set
            {
                _phoneNumber = value;
                OnPropertyChanged();
            }
        }


       public UserViewModel()
        {
            registerCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { Login(p); });
        }

        void Login(Window p)
        {
            if (p == null)
            {
                return ; 
            }
            if (!String.IsNullOrEmpty(_userName) && !String.IsNullOrEmpty(_password) && !String.IsNullOrEmpty(_fullName) && !String.IsNullOrEmpty(_phoneNumber))
            {
                isLogin = true;
                if (checkUserNameExisted(_userName))
                {
                    MessageBox.Show("Tên tài khoản đã tồn tại, vui lòng tạo tên tài khoản khác");
                }
                else
                { 
                
                }


                p.Close();
            }
            else {
                MessageBox.Show("Vui lòng điền đúng hoặc đầy đủ các thông tin");
            
            }
           
        }

        bool checkUserNameExisted(String input)
        {
            if (CoffeeXRepo.Ins.DB.UserInfoes.ToList().Any(x => x.username == input) )
            {
                return true;
            }
            return false;
        
        }


        static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }



    }
}
