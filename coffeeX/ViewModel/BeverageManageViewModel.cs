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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
            addCommand = new RelayCommand<AddBeverage>((p) => p != null, addBeverage);
            pickImageCommand = new RelayCommand<Object>((p) => true, pickImage);
            priceTextChanged = new RelayCommand<TextBox>((p) => true, validateTextBox);
              
        }

        private void validateTextBox(TextBox priceTextBox)
        {

            String txt = priceTextBox.Text;
            if (string.IsNullOrEmpty(txt))
                return;
            if (!txt.Contains("Đ"))
                priceTextBox.Text =priceTextBox.Text.Trim()+ " Đ";
            else
            {
                if(txt.Last() != 'Đ')
                {
                    txt = txt.Replace(" Đ", "").Trim();
                    txt = txt + " Đ";
                    priceTextBox.Text = txt;
                }

            }
        }
        private void addBeverage(AddBeverage addBeverageWindow)
        {/*
            if (string.IsNullOrEmpty(selectedBev.beverageName) || beverage.BeverageType.typeName is null
                || beverage.beveragePrice == 0 || beverage.beverageImage is null)
                return;
            MessageBox.Show(beverage.beverageName + "\n" + beverage.beveragePrice + "\n" + beverage.BeverageType.typeName);*/
            addBeverageWindow.toolTipTextBlock.Text = "xài ở viewmodel nè dcm mệt vl";
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
        public ICommand priceTextChanged { get; set; }


    }
}
