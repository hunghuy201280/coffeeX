using coffeeX.Model;
using coffeeX.Repository;
using coffeeX.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        private Table _currentTable;
        public Table currentTable
        {
            get => _currentTable; set
            {
                _currentTable = value;
                OnPropertyChanged();
            }
        }




        private MenuWindow menuWd;
        private ObservableCollection<BeverageType> _beverageType;
        public ObservableCollection<BeverageType> beverageType { get => _beverageType; set { _beverageType = value; OnPropertyChanged(); } }

        private ObservableCollection<List<Beverage>> _beverages;
        public ObservableCollection<List<Beverage>> beverages { get => _beverages; set { _beverages = value; OnPropertyChanged(); } }


        private ObservableCollection<Customer> _customerSuggest;

        public ObservableCollection<Customer> customerSuggest
        {
            get => _customerSuggest; set
            {
                _customerSuggest = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Voucher> _voucherSuggest;

        public ObservableCollection<Voucher> voucherSuggest
        {
            get => _voucherSuggest; set
            {
                _voucherSuggest = value;
                OnPropertyChanged();
            }
        }
        public MenuViewModel()
        {

            initBackgroundWorker();
            loadMenuWorker.RunWorkerAsync();
            suggestWorker.RunWorkerAsync();
            beverageType = new ObservableCollection<BeverageType>();
            beverages = new ObservableCollection<List<Beverage>>();
            
            onLoaded = new RelayCommand<MenuWindow>((p) => true, onWindowLoaded);

            deleteReceiptItemCmd = new RelayCommand<DataGrid>((p) => true, (p) =>
            {
                currentTable.receiptItems.Remove(p.SelectedItem as ReceiptDetail);
                new Thread(() =>
                {
                    currentTable.receiptValue = currentTable.receiptItems.Sum(it => it.total);
                }).Start();
            });
            cancelClickCmd = new RelayCommand<Window>((p) => true, onCancelClick);
            confirmClickCmd = new RelayCommand<MenuWindow>((p) => true, onConfirmClick);
            itemSelectedCmd = new RelayCommand<Beverage>((p) => true, MenuListBox_SelectionChanged);
            categoryListBoxSelectionChangedCmd = new RelayCommand<ListBox>((p) => true, CategoryListBox_SelectionChanged);
            increaseQuantity = new RelayCommand<DataGrid>((p) => true, (p) =>
            {
                (p.SelectedItem as ReceiptDetail).quantity++;
                new Thread(() =>
                {
                    currentTable.receiptValue = currentTable.receiptItems.Sum(it => it.total);
                }).Start();
            });
            decreaseQuantity = new RelayCommand<DataGrid>((p) => true, (p) =>
            {
                if ((p.SelectedItem as ReceiptDetail).quantity > 1)
                    (p.SelectedItem as ReceiptDetail).quantity--;
                new Thread(() =>
                {
                    currentTable.receiptValue = currentTable.receiptItems.Sum(it => it.total);
                }).Start();
            });


        }
        private void initBackgroundWorker()
        {
            loadMenuWorker = new BackgroundWorker();
            loadMenuWorker.DoWork += LoadMenuWorker_DoWork;
            loadMenuWorker.RunWorkerCompleted += LoadMenuWorker_RunWorkerCompleted;
            suggestWorker = new BackgroundWorker();
            suggestWorker.DoWork += SuggestWorker_DoWork;
            suggestWorker.RunWorkerCompleted += SuggestWorker_RunWorkerCompleted;
        }

        private void SuggestWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var res = e.Result as List<Object>;
            customerSuggest = new ObservableCollection<Customer>(res[0] as ObservableCollection<Customer>);
            voucherSuggest = new ObservableCollection<Voucher>(res[1] as ObservableCollection<Voucher>);
        }
        private void SuggestWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            using (CoffeeXEntities db=new CoffeeXEntities())
            {
                var tempCustomerSuggest = new ObservableCollection<Customer>(db.Customers);
                var tempVoucherSuggest = new ObservableCollection<Voucher>(db.Vouchers);
                e.Result = new List<Object>(new Object[] { tempCustomerSuggest, tempVoucherSuggest });
            }

        }

        private void LoadMenuWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var res = e.Result as List<Object>;
            beverageType = new ObservableCollection<BeverageType>(res[0] as ObservableCollection<BeverageType>);
            beverages = new ObservableCollection<List<Beverage>>(res[1] as ObservableCollection<List<Beverage>>);

        }

        private void LoadMenuWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<Beverage> allBeverages = CoffeeXRepo.getBeverages();
            var tempBeverageType = new ObservableCollection<BeverageType>(CoffeeXRepo.Ins.DB.BeverageTypes.ToList());
            var tempBeverages = new ObservableCollection<List<Beverage>>();
            foreach (var type in beverageType)
            {
                tempBeverages.Add(new List<Beverage>(type.Beverages));
            }
            var res = new List<Object>();
            res.Add(tempBeverageType);
            res.Add(tempBeverages);
            e.Result = res;
        }

        public ICommand onLoaded { get; set; }
        public ICommand deleteReceiptItemCmd { get; set; }
        public ICommand increaseQuantity { get; set; }
        public ICommand decreaseQuantity { get; set; }
        public ICommand categoryListBoxSelectionChangedCmd { get; set; }
        public ICommand itemSelectedCmd { get; set; }
        public ICommand cancelClickCmd { get; set; }
        public ICommand confirmClickCmd { get; set; }
        public BackgroundWorker loadMenuWorker;
        public BackgroundWorker suggestWorker;

        private void onCancelClick(Window p)
        {
            currentTable.resetStatus();
            p.Close();
        }
        private void onConfirmClick(MenuWindow p)
        {
            switch (currentTable.status)
            {
                case TableStatus.Free:
                    currentTable.status = TableStatus.Pending;
                    saveReceipt(p);
                    break;
                case TableStatus.Pending:
                    currentTable.status = TableStatus.Done;
                    break;
                case TableStatus.Done:
                    currentTable.resetStatus();

                    break;

            }
            p.Close();
        }

        private void saveReceipt(MenuWindow p)
        {
            Customer customer;
            Table table = currentTable;
            if (table.currentVoucher!=null && !String.IsNullOrEmpty(p.voucherTextBox.Text))
            {
                new NotifyPwdWindow("Voucher không hợp lệ").ShowDialog();
            }
            if (table.currentCustomer==null)
            {
                customer = new Customer()
                {
                    customerName = p.customerNameTextBox.Text,
                    phone = p.phoneTextBox.Text,
                };
                if(String.IsNullOrEmpty(customer.customerName)||String.IsNullOrEmpty(customer.phone))
                {
                    new NotifyPwdWindow("Vui lòng nhập thông tin khách hàng").ShowDialog();
                    return;
                }
            }
            else
            {
                customer = table.currentCustomer;
            }

            UserInfo currentStaff =( p.staffTextBox.DataContext as UserViewModel).currentUser;
            new Thread(() =>
            {
                CoffeeXRepo.addReceipt(customer, currentStaff, table.currentVoucher, table.receiptItems.ToList());
            }).Start();


        }

        private void phoneTextBoxTextChanged(MenuWindow menuWindow)
        {
            string phoneText = menuWindow.phoneTextBox.Text;
            new Thread(() =>
            {
                var customers = customerSuggest.Where(c => c.phone == phoneText).ToList();
                if (customers.Count > 0)
                {

                    menuWindow.customerNameTextBox.Dispatcher.Invoke(() =>
                    {
                        menuWindow.customerNameTextBox.Text = customers[0].customerName;
                    });
                }

            }).Start();
        }
        private void onWindowLoaded(MenuWindow menuWindow)
        {
            menuWd = menuWindow;
            currentTable = menuWd._currentTable;
        }

        private void MenuListBox_SelectionChanged(Beverage beverage)
        {


            if (beverage == null)
                return;
            menuWd.categoryListBox.SelectedItem = beverage.BeverageType;
            if (currentTable.receiptItems.Any(item => item.Beverage.beverageName == beverage.beverageName))
            {
                currentTable.receiptItems.Where(item => item.Beverage.beverageName == beverage.beverageName).ToList()[0].quantity++;
            }
            else
            {
                currentTable.receiptItems.Add(new ReceiptDetail()
                {
                    Beverage = beverage,
                    quantity = 1
                });
            
            }
            new Thread(() =>
            {
                currentTable.receiptValue = currentTable.receiptItems.Sum(it => it.total);
            }).Start();
        }

        private void CategoryListBox_SelectionChanged(ListBox listBox)
        {

            BeverageType selectedCategory = listBox.SelectedItem as BeverageType;
            int index = menuWd.menuListBox.Items.IndexOf(selectedCategory);

            if (index == -1)
                return;
            var container = menuWd.menuListBox.ItemContainerGenerator.ContainerFromItem(menuWd.menuListBox.Items[index]) as FrameworkElement;
            if (container != null)
                container.BringIntoView();
        }


    }
}
