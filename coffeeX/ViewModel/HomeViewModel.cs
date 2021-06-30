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
        public ICommand manageMenuCmd { get; set; }

        private ObservableCollection<int> _tableNumber;
        public ObservableCollection<int> tableNumber { get => _tableNumber; set { _tableNumber = value; OnPropertyChanged(); } }

        public HomeViewModel()
        {
            tableNumber = new ObservableCollection<int>();
            for (int i = 0; i < 40; i++)
                tableNumber.Add(i + 1);
            onLoaded = new RelayCommand<HomeWindow>((p) => true, onWindowLoaded);
            manageMenuCmd = new RelayCommand<HomeWindow>((p) => true, (p)=>new ChoiceBeverageManageDialog().ShowDialog());
      
        }

  
        private void onWindowLoaded(HomeWindow wd)
        {
            homeWd = wd;
        }
    }
}

