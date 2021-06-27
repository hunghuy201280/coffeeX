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
        } private string _currentBeverageName;

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
        private bool hasImage=false;
        public BeverageManageViewModel()
        {
            _beverageTypeSuggest = new ObservableCollection<string>(CoffeeXRepo.Ins.DB.BeverageTypes.Select((e) => e.typeName));
            addCommand = new RelayCommand<AddBeverage>(validateData, addBeverage);
            pickImageCommand = new RelayCommand<Object>((p) => true, pickImage);
            onLoaded = new RelayCommand<AddBeverage>((p) => true, onWindowLoaded);
           // priceTextChanged = new RelayCommand<Object>((p) => true, beveragePrice_KeyDown);
              
        }

   
        private void onWindowLoaded(AddBeverage addBeverage)
        {
            addBeverage.beveragePrice.PreviewTextInput += BeveragePrice_PreviewTextInput;
        }

        private void BeveragePrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

   

        private bool validateData(AddBeverage addBeverage)
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
            if (currentBeveragePrice==0)
                error += "Chưa có giá thức uống\n";
            if (!string.IsNullOrEmpty(error))
            {
           
                
                addBeverage.toolTipTextBlock.Text = error;
                return false;
            }
            

            addBeverage.toolTipTextBlock.Text = "";

            return true;
        }
        private void addBeverage(AddBeverage addBeverageWindow)
        {
            List<BeverageType> types=CoffeeXRepo.Ins.DB.BeverageTypes.Where(e => e.typeName == currentBeverageType).ToList();
            BeverageType type;

            if (types.Count==0)
            {

                type = new BeverageType();
                type.typeName = currentBeverageType;
                type=CoffeeXRepo.Ins.DB.BeverageTypes.Add(type);
                CoffeeXRepo.Ins.DB.SaveChanges();
                if (CoffeeXRepo.Ins.DB.Beverages.Any(b => b.beverageName == currentBeverageName))
                {
                    MessageBox.Show("Thức uống này đã tồn tại");
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
                if(CoffeeXRepo.Ins.DB.Beverages.Any(b=>b.beverageName==currentBeverageName))
                    {
                    MessageBox.Show("Thức uống này đã tồn tại");
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
            currentBeverageName =currentBeverageType ="";
            currentBeveragePrice = 0;
            currentBeverageImage = null;
            MessageBox.Show("Thêm món thành công!");


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


    }
}
