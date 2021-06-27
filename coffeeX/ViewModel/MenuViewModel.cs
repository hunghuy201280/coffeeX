using coffeeX.Model;
using coffeeX.Repository;
using coffeeX.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static coffeeX.View.MenuWindow;

namespace coffeeX.ViewModel
{
    public class MenuViewModel : BaseViewModel
    {

        private double _receiptValue;
        public double receiptValue
        {
            get => _receiptValue; set
            {
                _receiptValue = value;
                OnPropertyChanged();
            }
        }

        private MenuWindow menuWd;
        private ObservableCollection<BeverageType> _beverageType;
        public ObservableCollection<BeverageType> beverageType { get => _beverageType; set { _beverageType = value; OnPropertyChanged(); } }

        private ObservableCollection<List<Beverage>> _beverages;
        public ObservableCollection<List<Beverage>> beverages { get => _beverages; set { _beverages = value; OnPropertyChanged(); } }
        private ObservableCollection<ReceiptDetail> _receiptItems;
        public ObservableCollection<ReceiptDetail> receiptItems
        {
            get => _receiptItems; set
            {

                _receiptItems = value;

                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> _customerPhoneSuggest;

        public ObservableCollection<string> customerPhoneSuggest
        {
            get => _customerPhoneSuggest; set
            {
                _customerPhoneSuggest = value;
                OnPropertyChanged();
            }
        }

        public MenuViewModel()
        {
            LoadMenu();
            _customerPhoneSuggest = new ObservableCollection<string>(CoffeeXRepo.Ins.DB.Customers.Select((e) => e.phone));

            receiptItems = new ObservableCollection<ReceiptDetail>();
            onLoaded = new RelayCommand<MenuWindow>((p) => true, onWindowLoaded);

            deleteReceiptItemCmd = new RelayCommand<DataGrid>((p) => true, (p) =>
            {
                receiptItems.Remove(p.SelectedItem as ReceiptDetail);
                new Thread(() =>
                {
                    receiptValue = _receiptItems.Sum(it => it.total);
                }).Start();
            });
            phoneTextBoxTextChangedCmd = new RelayCommand<MenuWindow>((p) => true, phoneTextBoxTextChanged);
            itemSelectedCmd = new RelayCommand<Beverage>((p) => true, MenuListBox_SelectionChanged);
            categoryListBoxSelectionChangedCmd = new RelayCommand<ListBox>((p) => true, CategoryListBox_SelectionChanged);
            increaseQuantity = new RelayCommand<DataGrid>((p) => true, (p) =>
            {
                (p.SelectedItem as ReceiptDetail).quantity++;
                new Thread(() =>
                {
                    receiptValue = _receiptItems.Sum(it => it.total);
                }).Start();
            });
            decreaseQuantity = new RelayCommand<DataGrid>((p) => true, (p) =>
            {
                if ((p.SelectedItem as ReceiptDetail).quantity > 1)
                    (p.SelectedItem as ReceiptDetail).quantity--;
                new Thread(() =>
                {
                    receiptValue = _receiptItems.Sum(it => it.total);
                }).Start();
            });


        }
        public ICommand onLoaded { get; set; }
        public ICommand deleteReceiptItemCmd { get; set; }
        public ICommand increaseQuantity { get; set; }
        public ICommand decreaseQuantity { get; set; }
        public ICommand categoryListBoxSelectionChangedCmd { get; set; }
        public ICommand itemSelectedCmd { get; set; }
        public ICommand phoneTextBoxTextChangedCmd { get; set; }


        private void phoneTextBoxTextChanged(MenuWindow menuWindow)
        {
            string phoneText = menuWindow.phoneTextBox.Text;
            new Thread(() =>
            {
                using(var db=new CoffeeXEntities())
                {
                    if (db.Customers.Any(c => c.phone == phoneText))
                    {
                        Customer customer = db.Customers.Where(c => c.phone == phoneText).ToList().First();
                        menuWindow.customerNameTextBox.Dispatcher.Invoke(() =>
                        {
                            menuWindow.customerNameTextBox.Text = customer.customerName;
                        });
                    }
                }
              
            }).Start();
          
        }
        private void onWindowLoaded(MenuWindow menuWindow)
        {
            menuWd = menuWindow;
        }

        private void MenuListBox_SelectionChanged(Beverage beverage)
        {


            if (beverage == null)
                return;
            menuWd.categoryListBox.SelectedItem = beverage.BeverageType;
            if (receiptItems.Any(item => item.Beverage.beverageName == beverage.beverageName))
            {
                receiptItems.Where(item => item.Beverage.beverageName == beverage.beverageName).ToList()[0].quantity++;
            }
            else
            {
                receiptItems.Add(new ReceiptDetail()
                {
                    Beverage = beverage,
                    quantity = 1
                });
                new Thread(() =>
                {
                    receiptValue = _receiptItems.Sum(it => it.total);
                }).Start();
            }
        }

        private void CategoryListBox_SelectionChanged(ListBox listBox)
        {

            BeverageType selectedCategory = listBox.SelectedItem as BeverageType;
            int index = menuWd.menuListBox.Items.IndexOf(selectedCategory);
            //menuWd.menuListBox.ScrollIntoView(menuWd.menuListBox.Items[index]);
            /*  var point = menuWd.TranslatePoint(new Point() - (Vector)e.GetPosition(sv), ip);
              sv.ScrollToVerticalOffset(point.Y + (item.ActualHeight / 2));*/
            if (index == -1)
                return;
            var container = menuWd.menuListBox.ItemContainerGenerator.ContainerFromItem(menuWd.menuListBox.Items[index]) as FrameworkElement;
            if (container != null)
                container.BringIntoView();
        }

        private void LoadMenu()
        {
            List<Beverage> allBeverages = CoffeeXRepo.Ins.DB.Beverages.ToList();
            beverageType = new ObservableCollection<BeverageType>(CoffeeXRepo.Ins.DB.BeverageTypes.ToList());
            beverages = new ObservableCollection<List<Beverage>>();
            foreach (var type in beverageType)
            {
                beverages.Add(new List<Beverage>(type.Beverages));
            }
        }

    }
}
