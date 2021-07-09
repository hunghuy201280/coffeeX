using coffeeX.Model;
using coffeeX.Repository;
using coffeeX.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace coffeeX.ViewModel
{
    class IngredientManageViewModel : BaseViewModel
    {

        CoffeeXEntities db = new CoffeeXEntities();
        public IngredientManageViewModel()
        {
            initVariable();
            initCmd();
            initWorker();
            loadIngredients();
        }

        private void initVariable()
        {
            _ingredientUnitSuggest = new ObservableCollection<String>();
            _ingredients = new ObservableCollection<Ingredient>();
        }

        private void initWorker()
        {
            suggestWorker = new BackgroundWorker();
            suggestWorker.RunWorkerCompleted += SuggestWorker_RunWorkerCompleted;
            suggestWorker.DoWork += SuggestWorker_DoWork;
            ingredientWorker = new BackgroundWorker();
            ingredientWorker.DoWork += IngredientWorker_DoWork;
            ingredientWorker.RunWorkerCompleted += IngredientWorker_RunWorkerCompleted;
        }

        private void IngredientWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ingredients.Clear();
            (e.Result as List<Ingredient>).ForEach(ingredients.Add);
        }

        private void IngredientWorker_DoWork(object sender, DoWorkEventArgs e)
        {
        
           
                List<Ingredient> res = db.Ingredients.ToList();
                e.Result = res;
            
          
        }

        private void SuggestWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<String> units = CoffeeXRepo.Ins.DB.Units.Select(it => it.unitName).ToList();
            e.Result = units;
        }

        private void SuggestWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ingredientUnitSuggest.Clear();
            (e.Result as List<String>).ForEach(ingredientUnitSuggest.Add);
        }

        private void initCmd()
        {
            onAddLoadedCmd = new RelayCommand<AddIngredientWindow>((p) => p != null, onAddIngredientWindowLoaded);
            onUpdateIngredientWindowLoadedCmd = new RelayCommand<UpdateIngredientWindow>((p) => p != null, onUpdateIngredientWindowLoaded);
            onAddClickCmd = new RelayCommand<AddIngredientWindow>((p) => p != null, onAddIngredientClick);
            deleteIngredientCmd = new RelayCommand<Ingredient>((p) => p != null, onDeleteIngredientClick);
            ingredientSelectedChanged = new RelayCommand<Ingredient>((p) => p != null, onIngredientSelectedChanged);
            updateIngredientCmd = new RelayCommand<UpdateIngredientWindow>((p) => p != null, onIngredientUpdateClick);

        }
        public delegate void reLoadPaymentIngredientDelegate();
        reLoadPaymentIngredientDelegate reLoadIngredient;

        private void onUpdateIngredientWindowLoaded(UpdateIngredientWindow obj)
        {
            currentIngredientName = currentIngredientUnit = "";
            currentIngredientPrice = 0;
            reLoadIngredient = (obj.PaymentVM.DataContext as PaymentViewModel).loadData;
            obj.ingredientPrice.PreviewTextInput += IngredientPrice_PreviewTextInput;
            loadUnitSuggestion();
        }

        private void IngredientPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private void onIngredientUpdateClick(UpdateIngredientWindow updateIngredientWindow)
        {
            if (String.IsNullOrEmpty(_currentIngredientName) || String.IsNullOrEmpty(_currentIngredientUnit) || _currentIngredientPrice == 0)
            {
                new NotifyPwdWindow("Vui lòng nhập đầy đủ thông tin !").ShowDialog();
                return;
            }
            Unit ingredientUnit = db.Units.SingleOrDefault(it => it.unitName.ToLower().Equals(_currentIngredientUnit.ToLower()));
            if (ingredientUnit == null)
            {
                ingredientUnit = new Unit()
                {
                    unitName = _currentIngredientUnit.Trim(),
                };
            }
            Ingredient newIngredient = new Ingredient()
            {
                ingredientName = _currentIngredientName.Trim(),
                ingredientPrice = _currentIngredientPrice,
                Unit = ingredientUnit,
            };
            Ingredient updateIngredient = db.Ingredients.SingleOrDefault(it => it.ingredientID == selectedIngredientID);
            if (updateIngredient != null)
            {
                updateIngredient.ingredientName = currentIngredientName.Trim();
                updateIngredient.ingredientPrice = _currentIngredientPrice;
                updateIngredient.Unit = ingredientUnit;
                db.SaveChanges();
                var temp= ingredients.SingleOrDefault(it => it.ingredientID == updateIngredient.ingredientID);
                if(temp!=null)
                {
                    temp.ingredientName = updateIngredient.ingredientName;
                    temp.ingredientPrice = updateIngredient.ingredientPrice;
                    temp.Unit = ingredientUnit;
                }

                new NotifyPwdWindow("Cập nhật nguyên liệu thành công !").ShowDialog();
                reLoadIngredient();

            }



        }

        private void onIngredientSelectedChanged(Ingredient obj)
        {
            selectedIngredientID = obj.ingredientID;
            currentIngredientName = obj.ingredientName;
            currentIngredientPrice = obj.ingredientPrice;
            currentIngredientUnit = obj.Unit.unitName;
        }

        private void onDeleteIngredientClick(Ingredient obj)
        {
            var x = db.Ingredients.Where(p => p.ingredientID == selectedIngredientID).SingleOrDefault();
            db.Ingredients.Remove(x);
            var rees=ingredients.Remove(x);
            db.SaveChanges();
            onAddIngredientWindowLoaded(null);
            NotifyPwdWindow notifyWindow = new NotifyPwdWindow("Xóa món thành công");
            notifyWindow.ShowDialog();

            reLoadIngredient();
        }

        private void onAddIngredientClick(AddIngredientWindow addIngredientWindow)
        {
            if (String.IsNullOrEmpty(_currentIngredientName) || String.IsNullOrEmpty(_currentIngredientUnit) || _currentIngredientPrice == 0)
            {
                new NotifyPwdWindow("Vui lòng nhập đầy đủ thông tin !").ShowDialog();
                return;
            }
            if (CoffeeXRepo.Ins.DB.Ingredients.Any(it => it.ingredientName.ToLower().Equals(_currentIngredientName.ToLower().Trim())))
            {
                new NotifyPwdWindow("Nguyên liệu đã tồn tại !").ShowDialog();
                return;
            }
            Unit ingredientUnit = CoffeeXRepo.Ins.DB.Units.SingleOrDefault(it => it.unitName.ToLower().Equals(_currentIngredientUnit.ToLower()));
            if (ingredientUnit == null)
            {
                ingredientUnit = new Unit()
                {
                    unitName = _currentIngredientUnit.Trim(),
                };
            }
            Ingredient newIngredient = new Ingredient()
            {
                ingredientName = _currentIngredientName.Trim(),
                ingredientPrice = _currentIngredientPrice,
                Unit = ingredientUnit,
            };
            CoffeeXRepo.Ins.DB.Ingredients.Add(newIngredient);
            CoffeeXRepo.Ins.DB.SaveChanges();

            onAddIngredientWindowLoaded(null);
            new NotifyPwdWindow("Thêm nguyên liệu thành công!").ShowDialog();
            (addIngredientWindow.PaymentVM.DataContext as PaymentViewModel).loadData();
            loadIngredients();
        }

        private void onAddIngredientWindowLoaded(AddIngredientWindow obj)
        {
            currentIngredientName = currentIngredientUnit = "";
            currentIngredientPrice = 0;
            obj.ingredientPrice.PreviewTextInput += IngredientPrice_PreviewTextInput;
            loadUnitSuggestion();
        }
        private void loadUnitSuggestion()
        {
            if (!suggestWorker.IsBusy)
                suggestWorker.RunWorkerAsync();
        }
        private void loadIngredients()
        {
            if (!ingredientWorker.IsBusy)
                ingredientWorker.RunWorkerAsync();
        }

        private int selectedIngredientID;
        private BackgroundWorker suggestWorker;
        private BackgroundWorker ingredientWorker;
        public ICommand onAddLoadedCmd { get; set; }
        public ICommand onUpdateIngredientWindowLoadedCmd { get; set; }
        public ICommand ingredientSelectedChanged { get; set; }
        public ICommand onAddClickCmd { get; set; }
        public ICommand deleteIngredientCmd { get; set; }
        public ICommand updateIngredientCmd { get; set; }
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

        private ObservableCollection<Ingredient> _ingredients;
        public ObservableCollection<Ingredient> ingredients
        {
            get => _ingredients; set
            {
                _ingredients = value;
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
