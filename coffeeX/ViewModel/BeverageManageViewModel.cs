using coffeeX.Model;
using coffeeX.Repository;
using coffeeX.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;


namespace coffeeX.ViewModel
{
    class BeverageManageViewModel : BaseViewModel
    {
    
 
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
        public BeverageManageViewModel()
        {
            _beverageTypeSuggest = new ObservableCollection<string>(CoffeeXRepo.Ins.DB.BeverageTypes.Select((e) => e.typeName));
            addCommand = new RelayCommand<Beverage>((p) => p != null, addBeverage);
            pickImageCommand = new RelayCommand<Object>((p) => true, pickImage);

        }

        private void addBeverage(Beverage beverage)
        {
            if (string.IsNullOrEmpty(beverage.beverageName) || beverage.BeverageType.typeName is null
                || beverage.beveragePrice == 0 || beverage.beverageImage is null)
                return;
            MessageBox.Show(beverage.beverageName + "\n" + beverage.beveragePrice + "\n" + beverage.BeverageType.typeName);
        }
        private void pickImage(Object p)
        {
            imagePicker = new OpenFileDialog();
            imagePicker.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if ((bool)imagePicker.ShowDialog())
            {
              /*  Beverage temp = selectedBeverage;
                temp.beverageImage = ImageDataConverter.ConvertBitmapSourceToByteArray(imagePicker.FileName);
                selectedBeverage = temp;*/
            }

        }
        public ICommand addCommand { get; set; }
        public ICommand pickImageCommand { get; set; }


    }
}
