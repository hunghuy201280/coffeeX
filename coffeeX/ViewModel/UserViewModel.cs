using coffeeX.Model;
using coffeeX.Repository;
using coffeeX.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace coffeeX.ViewModel
{

    public class UserViewModel : BaseViewModel
    {
        public ICommand registerCommand { get; set; }
        public ICommand passwordChangedCommand { get; set; }
        public ICommand loginCommand { get; set; }
        public ICommand registerClickCommand { get; set; }

        public ICommand logOutCmd { get; set; }



        private UserInfo _currentUser;

        public UserInfo currentUser { get => _currentUser; set { _currentUser = value; OnPropertyChanged(); } }

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


        private string _tooltip;
        public string tooltip { get => _tooltip; set { _tooltip = value; OnPropertyChanged(); } }

        


        public UserViewModel()
        {
            fullName = phoneNumber = userName = password= "";
            registerCommand = new RelayCommand<Window>((p) => { return validRegis(); }, register);
            passwordChangedCommand = new RelayCommand<TextBox>((p) => { return true; }, (p) => { password = p.Text; });
            loginCommand = new RelayCommand<Window>((p) => { return checkEmptyUserIDPassword(); },  login);
            registerClickCommand = new RelayCommand<Object>((p) =>true,  (p)=>new RegisterWindow().Show());
            logOutCmd= new RelayCommand<Window>((p) => p!=null, onLogOutCleanup);
        }


        private void onLogOutCleanup(Window p)
        {
            currentUser = null;
            userName = password = "";
            isLogin = false;
            new LoginWindow().Show();
            p.Close();


        }

        private bool validRegis()
        {
            var passWordValidate = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$");
            var userNameValidate = new Regex(@"^[a-zA-Z0-9_-]{3,16}$");
            var phoneValidate = new Regex(@"(84|0[3|5|7|8|9])+([0-9]{8})\b");
            bool result = true;
            tooltip = "";
            if (fullName.Trim() == string.Empty)
            {
                tooltip = "Tên chưa đúng";
                result = false;
            }
            else
            if (!userNameValidate.IsMatch(userName))
            {
                tooltip = "UserName có từ 3 đên 16 ký tự";
                result = false;
            }
            else
            if(!phoneValidate.IsMatch(phoneNumber))
            {
                tooltip = "Số điện thoại không phù hợp";
                result = false;
            }
           
            else
            if (!passWordValidate.IsMatch(password))
            {
                tooltip = "Password phải từ 8 ký tự đổ lên(Ít nhất 1 ký tự viết hoa ,1 ký tự viết thường ,1 số)";
                result = false;
            }
            if (result) tooltip = "Có thể đăng ký";
            return result;

        }


        bool checkEmptyUserIDPassword()
        {
            if (!String.IsNullOrEmpty(userName) && !String.IsNullOrEmpty(password))
                return true;
            return false;
        }


        void login(Window p)
        {
            if (p == null)
            {
                return;
            }
            if (checkNameAndPassword(userName, password))
            {
                isLogin = true;
                new HomeWindow().Show();
                p.Close();
            }
            else
            {
               
                NotifyPwdWindow notifyWindow = new NotifyPwdWindow("Tài khoản hoặc mật khẩu không đúng");
                notifyWindow.ShowDialog();
            }

        }


        void register(Window p)
        {
            if (p == null)
            {
                return ; 
            }
            if (!String.IsNullOrEmpty(_userName) && !String.IsNullOrEmpty(_password) && !String.IsNullOrEmpty(_fullName) && !String.IsNullOrEmpty(_phoneNumber))
            {
                
                if (checkUserNameExisted(_userName))
                {
                   
                    NotifyPwdWindow notifyWindow = new NotifyPwdWindow("Tên tài khoản đã tồn tại, vui lòng tạo tên tài khoản khác");
                    notifyWindow.ShowDialog();

                }
                else
                {
                    
                    UserInfo newUser = new UserInfo() {fullName= fullName,phoneNumber= phoneNumber,username=userName,passwordEncrypted=ComputeSha256Hash(password),roleID=1};
                    CoffeeXRepo.Ins.DB.UserInfoes.Add(newUser);
                    CoffeeXRepo.Ins.DB.SaveChanges();

                   isLogin = true;
                    p.Close();
                }


                
            }
            else {
           
                NotifyPwdWindow notifyWindow = new NotifyPwdWindow("Vui lòng điền đúng hoặc đầy đủ các thông tin");
                notifyWindow.ShowDialog();

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


        bool checkNameAndPassword(String name, String password)
        {
            var results = CoffeeXRepo.Ins.DB.UserInfoes.ToList().Where(x => x.username == name && x.passwordEncrypted == ComputeSha256Hash(password)).ToList();
            if (results.Count>0)
            {
                currentUser = results[0];
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
