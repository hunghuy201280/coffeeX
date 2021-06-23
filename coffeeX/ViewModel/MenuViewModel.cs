using coffeeX.Model;
using coffeeX.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static coffeeX.View.MenuWindow;

namespace coffeeX.ViewModel
{
    public class MenuViewModel : BaseViewModel
    {


        private ObservableCollection<BeverageType> _beverageType;
        public ObservableCollection<BeverageType> beverageType { get=> _beverageType; set { _beverageType = value; OnPropertyChanged(); } }
        
        private ObservableCollection<List<Beverage>> _beverages;
        public ObservableCollection<List<Beverage>> beverages { get=> _beverages; set { _beverages = value; OnPropertyChanged(); } }

        public MenuViewModel()
        {
            LoadMenu();
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
