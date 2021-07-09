using coffeeX.Model;
using coffeeX.Repository;
using coffeeX.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace coffeeX.ViewModel
{
    class IngredientManageViewModel : BaseViewModel
    {

        public IngredientManageViewModel()
        {
            initVariable();
            initCmd();
            initWorker();
        }

        private void initVariable()
        {
            _ingredientUnitSuggest = new ObservableCollection<String>();
        }

        private void initWorker()
        {
            suggestWorker = new BackgroundWorker();
            suggestWorker.RunWorkerCompleted += SuggestWorker_RunWorkerCompleted;
            suggestWorker.DoWork += SuggestWorker_DoWork;
        }

        private void SuggestWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<String> units = CoffeeXRepo.Ins.DB.Units.Select(it=>it.unitName).ToList();
            e.Result = units;
        }

        private void SuggestWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            (e.Result as List<String>).ForEach(ingredientUnitSuggest.Add);
        }

        private void initCmd()
        {
            onAddLoadedCmd = new RelayCommand<AddIngredientWindow>((p) => p != null, onAddIngredientWindowLoaded);
            onAddClickCmd = new RelayCommand<AddIngredientWindow>((p) => p!=null, onAddIngredientClick);
        }

        private void onAddIngredientClick(AddIngredientWindow addIngredientWindow)
        {
            if(String.IsNullOrEmpty(_currentIngredientName)|| String.IsNullOrEmpty(_currentIngredientUnit) || _currentIngredientPrice==0)
            {
                new NotifyPwdWindow("Vui lòng nhập đầy đủ thông tin !").ShowDialog();
                return;
            }
            if(CoffeeXRepo.Ins.DB.Ingredients.Any(it=>it.ingredientName.ToLower().Equals(_currentIngredientName.ToLower().Trim())))
            {
                new NotifyPwdWindow("Nguyên liệu đã tồn tại !").ShowDialog();
                return;
            }
            Unit ingredientUnit = CoffeeXRepo.Ins.DB.Units.SingleOrDefault(it => it.unitName.ToLower().Equals(_currentIngredientUnit.ToLower()));
            if(ingredientUnit==null)
            {
                ingredientUnit = new Unit()
                {
                    unitName = _currentIngredientUnit.Trim(),
                };
            }
            Ingredient newIngredient = new Ingredient()
            {
                ingredientName=_currentIngredientName.Trim(),
                ingredientPrice=_currentIngredientPrice,
                Unit=ingredientUnit,
            };
            CoffeeXRepo.Ins.DB.Ingredients.Add(newIngredient);
            CoffeeXRepo.Ins.DB.SaveChanges();

            onAddIngredientWindowLoaded(null);
            new NotifyPwdWindow("Thêm nguyên liệu thành công!").ShowDialog();
            (addIngredientWindow.PaymentVM.DataContext as PaymentViewModel).loadData();
        }

        private void onAddIngredientWindowLoaded(AddIngredientWindow obj)
        {
            currentIngredientName = currentIngredientUnit = "";
            currentIngredientPrice = 0;
            loadUnitSuggestion();
            
        }
        private void loadUnitSuggestion()
        {
            if (!suggestWorker.IsBusy)
                suggestWorker.RunWorkerAsync();
        }

        private BackgroundWorker suggestWorker;
        public ICommand onAddLoadedCmd { get; set; }
        public ICommand onAddClickCmd { get; set; }
        private String _currentIngredientName;
        public String currentIngredientName
        {
            get => _currentIngredientName; set
            {
                _currentIngredientName = value;
                OnPropertyChanged();
            }
        }
        private String _currentIngredientUnit;
        public String currentIngredientUnit
        {
            get => _currentIngredientUnit; set
            {
                _currentIngredientUnit = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<String> _ingredientUnitSuggest;
        public ObservableCollection<String> ingredientUnitSuggest
        {
            get => _ingredientUnitSuggest; set
            {
                _ingredientUnitSuggest = value;
                OnPropertyChanged();
            }
        }
        
        private Double _currentIngredientPrice;
        public Double currentIngredientPrice
        {
            get => _currentIngredientPrice; set
            {
                _currentIngredientPrice = value;
                OnPropertyChanged();
            }
        }

    }
}
