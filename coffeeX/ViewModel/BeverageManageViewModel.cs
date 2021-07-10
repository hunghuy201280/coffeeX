using coffeeX.Model;
using coffeeX.Repository;
using coffeeX.Utils;
using coffeeX.View;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace coffeeX.ViewModel
{
    class BeverageManageViewModel : BaseViewModel
    {

        private Byte[] _currentBeverageImage;

        public Byte[] currentBeverageImage
        {
            get => _currentBeverageImage; set
            {
                _currentBeverageImage = value;
                OnPropertyChanged();
            }
        }

        private string _currentBeverageType;

        public string currentBeverageType
        {
            get => _currentBeverageType; set
            {
                _currentBeverageType = value;
                OnPropertyChanged();
            }
        }

        private string _currentBeverageName;

        public string currentBeverageName
        {
            get => _currentBeverageName; set
            {
                _currentBeverageName = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<string> _beverageTypeSuggest;

        public ObservableCollection<string> beverageTypeSuggest
        {
            get => _beverageTypeSuggest; set
            {
                _beverageTypeSuggest = value;
                OnPropertyChanged();
            }
        }

        private double _currentBeveragePrice;

        public double currentBeveragePrice
        {
            get => _currentBeveragePrice; set
            {
                _currentBeveragePrice = value;
                OnPropertyChanged();
            }
        }


        private OpenFileDialog imagePicker;
        private bool hasImage = false;
        public BeverageManageViewModel()
        {
            loadbeverage();

            initCmd();

        }

        private void initCmd()
        {
            _beverageTypeSuggest = new ObservableCollection<string>(CoffeeXRepo.Ins.DB.BeverageTypes.Select((e) => e.typeName));
            addCommand = new RelayCommand<AddBeverageWindow>(validateData, addBeverage);
            pickImageCommand = new RelayCommand<Object>((p) => true, pickImage);
            onLoaded = new RelayCommand<AddBeverageWindow>((p) => true, onWindowLoaded);
            // priceTextChanged = new RelayCommand<Object>((p) => true, beveragePrice_KeyDown);
            chooseBeverage = new RelayCommand<Beverage>((p) => p != null, p =>
            {
                addToTextBox(p);
            });
            onUpdateBeverageWindowLoaded = new RelayCommand<UpdateBeverageWindow>((p) => true, onUpdateWindowLoaded);
            deleteBeverageCmd = new RelayCommand<Object>((p) => true, deleteBeverage);
            updateBeverageCmd = new RelayCommand<Object>((p) => true, updateBeverage);
        }

        private void onUpdateWindowLoaded(UpdateBeverageWindow updateBeverage)
        {
            currentBeverageName = currentBeverageType = "";
            currentBeveragePrice = 0;
            currentBeverageImage = null;
        }

        private void onWindowLoaded(AddBeverageWindow addBeverage)
        {
            currentBeverageName = currentBeverageType = "";
            currentBeveragePrice = 0;
            currentBeverageImage = null;
            addBeverage.beveragePrice.PreviewTextInput += BeveragePrice_PreviewTextInput;

        }

        private void BeveragePrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }



        private bool validateData(AddBeverageWindow addBeverage)
        {
            if (addBeverage == null)
                return false;
            String error = "";
            if (!hasImage)
                error += "Chưa có ảnh thức uống\n";
            if (string.IsNullOrEmpty(currentBeverageName))
                error += "Chưa có tên thức uống\n";
            if (string.IsNullOrEmpty(currentBeverageType))
                error += "Chưa có loại thức uống\n";
            if (currentBeveragePrice == 0)
                error += "Chưa có giá thức uống\n";
            if (!string.IsNullOrEmpty(error))
            {


                addBeverage.toolTipTextBlock.Text = error;
                return false;
            }


            addBeverage.toolTipTextBlock.Text = "";

            return true;
        }
        private void addBeverage(AddBeverageWindow addBeverageWindow)
        {
            List<BeverageType> types = CoffeeXRepo.Ins.DB.BeverageTypes.Where(e => e.typeName == currentBeverageType).ToList();
            BeverageType type;

            if (types.Count == 0)
            {
                type = new BeverageType();
                type.typeName = currentBeverageType;
                type = CoffeeXRepo.Ins.DB.BeverageTypes.Add(type);
                CoffeeXRepo.Ins.DB.SaveChanges();
                if (CoffeeXRepo.Ins.DB.Beverages.Any(b => b.beverageName == currentBeverageName))
                {
                    new NotifyPwdWindow("Thức uống này đã tồn tại").ShowDialog();
                
                
                    return;
                }
                Beverage newBeverage = new Beverage();
                newBeverage.beverageImage = currentBeverageImage;
                newBeverage.beverageName = currentBeverageName;
                newBeverage.beveragePrice = currentBeveragePrice;
                newBeverage.BeverageType = type;
                CoffeeXRepo.Ins.DB.Beverages.Add(newBeverage);
                CoffeeXRepo.Ins.DB.SaveChanges();
            }
            else
            {
                type = types.First();
                if (CoffeeXRepo.Ins.DB.Beverages.Any(b => b.beverageName == currentBeverageName))
                {
                    new NotifyPwdWindow("Thức uống này đã tồn tại").ShowDialog();


                    return;
                }
                Beverage newBeverage = new Beverage();
                newBeverage.beverageImage = currentBeverageImage;
                newBeverage.beverageName = currentBeverageName;
                newBeverage.beveragePrice = currentBeveragePrice;
                newBeverage.BeverageType = type;
                CoffeeXRepo.Ins.DB.Beverages.Add(newBeverage);
                CoffeeXRepo.Ins.DB.SaveChanges();
            }
            currentBeverageName = currentBeverageType = "";
            currentBeveragePrice = 0;
            currentBeverageImage = null;
            (addBeverageWindow.menuViewModel.DataContext as MenuViewModel).loadMenu();
            loadbeverage();
            NotifyPwdWindow notifyWindow = new NotifyPwdWindow("Thêm món thành công");
            notifyWindow.ShowDialog();


        }
        private void pickImage(Object p)
        {
            imagePicker = new OpenFileDialog();
            imagePicker.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if ((bool)imagePicker.ShowDialog())
            {

                currentBeverageImage = ImageDataConverter.ConvertBitmapSourceToByteArray(imagePicker.FileName);
                hasImage = true;
            }

        }
        public ICommand addCommand { get; set; }
        public ICommand pickImageCommand { get; set; }
        public ICommand priceTextChanged { get; set; }
        public ICommand onLoaded { get; set; }
        public ICommand onUpdateBeverageWindowLoaded { get; set; }
        public ICommand chooseBeverage { get; set; }
        public ICommand deleteBeverageCmd { get; set; }
        public ICommand updateBeverageCmd { get; set; }

        private ObservableCollection<Beverage> _updatedBeverage;
        public ObservableCollection<Beverage> updatedBeverage
        {
            get => this._updatedBeverage; set
            {
                this._updatedBeverage = value;
                OnPropertyChanged();

            }
        }

        public void loadbeverage()
        {
            if (updatedBeverage == null)
                updatedBeverage = new ObservableCollection<Beverage>();

            updatedBeverage.Clear();
            try
            {
                var tempBeverage = CoffeeXRepo.Ins.DB.Beverages.ToList();

                var tempType = CoffeeXRepo.Ins.DB.BeverageTypes.ToList();

                foreach (Beverage i in tempBeverage)
                {
                    var temp = tempType.Where(o => i.typeID == o.typeID).ToList();

                    BeverageType type = temp[0];
                    i.BeverageType = type;
                    updatedBeverage.Add(i);
                }


            }
            catch
            {

            }
        }

        int beverageId;

        void addToTextBox(Beverage input)
        {
            beverageId = input.beverageID;
            currentBeverageName = input.beverageName;
            currentBeveragePrice = input.beveragePrice;
            currentBeverageType = input.BeverageType.typeName;
            currentBeverageImage = input.beverageImage;

        }


        void deleteBeverage(Object o)
        {

            var x = CoffeeXRepo.Ins.DB.Beverages.Where(p => p.beverageName == currentBeverageName).SingleOrDefault();
            CoffeeXRepo.Ins.DB.Beverages.Remove(x);
            updatedBeverage.Remove(x);
            CoffeeXRepo.Ins.DB.SaveChanges();
            currentBeverageName = currentBeverageType = "";
            currentBeveragePrice = 0;
            currentBeverageImage = null;
            NotifyPwdWindow notifyWindow = new NotifyPwdWindow("Xóa món thành công");
            notifyWindow.ShowDialog();

        }

        void updateBeverage(Object p)
        {
            if (String.IsNullOrEmpty(currentBeverageName) || currentBeveragePrice == 0 || String.IsNullOrEmpty(currentBeverageType) || currentBeverageImage == null)
            {
                NotifyPwdWindow notifyWindow = new NotifyPwdWindow("Vui lòng điền đầy đủ các thông tin");
                notifyWindow.ShowDialog();
            }
            else
            {
          
                var temp = CoffeeXRepo.Ins.DB.BeverageTypes.Where(x => x.typeName == currentBeverageType).FirstOrDefault();

                var result = CoffeeXRepo.Ins.DB.Beverages.SingleOrDefault(b => b.beverageID == beverageId);
                if (result != null)
                {
                    result.beverageName = currentBeverageName;
                    result.beveragePrice = currentBeveragePrice;
                    result.beverageImage = currentBeverageImage;
                    result.BeverageType = temp;
                    CoffeeXRepo.Ins.DB.SaveChanges();

                }

                var obj = updatedBeverage.FirstOrDefault(x => x.beverageID == beverageId);
                if (obj != null)
                {
                    obj.beverageName = currentBeverageName;
                    obj.beveragePrice = currentBeveragePrice;
                    obj.beverageImage = currentBeverageImage;
                    obj.BeverageType = temp;

                }

                currentBeverageName = currentBeverageType = "";
                currentBeveragePrice = 0;
                currentBeverageImage = null;
                NotifyPwdWindow notifyWindow = new NotifyPwdWindow("Chỉnh sửa món thành công");
                notifyWindow.ShowDialog();



            }

        }

    }
}
