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
using static coffeeX.View.HomeWindow;

namespace coffeeX.ViewModel
{
    public class HomeViewModel : BaseViewModel
    {
        private HomeWindow homeWd;
        public ICommand onLoaded { get; set; }
        public ICommand addBeverageCmd { get; set; }
        public ICommand modifyBeverageCmd { get; set; }
        public ICommand onTableClickCmd { get; set; }
        public ICommand onStatisticClickCmd { get; set; }
        public ICommand addPaymentVoucherCmd { get; set; }

        public ICommand changePwdCmd { get; set; }

        private ObservableCollection<Table> _table;
        public ObservableCollection<Table> table { get => _table; set { _table = value; OnPropertyChanged(); } }

        public HomeViewModel()
        {
            table = new ObservableCollection<Table>();
            for (int i = 0; i < 40; i++)
                table.Add(new Table(i + 1) ) ;
            onLoaded = new RelayCommand<HomeWindow>((p) => true, OnWindowLoaded);
            onStatisticClickCmd = new RelayCommand<Object>((p) => true, (p)=>new StatisticWindow().ShowDialog());
            addBeverageCmd = new RelayCommand<Object>((p) => true, (p)=>new AddBeverageWindow().ShowDialog());
            modifyBeverageCmd = new RelayCommand<Object>((p) => true, (p)=>new UpdateBeverageWindow().ShowDialog());
            addPaymentVoucherCmd = new RelayCommand<Object>((p) => true, (p)=>new PaymentWindow().ShowDialog());
            onTableClickCmd = new RelayCommand<Table>((p) => true, (p)=> {
     
                new MenuWindow(p).ShowDialog();
             
            });
            changePwdCmd = new RelayCommand<Object>((p) => true, (p) => new ChangePwdWindow().ShowDialog());

        }

  
        private void OnWindowLoaded(HomeWindow wd)
        {
            homeWd = wd;
        }
    }
}

