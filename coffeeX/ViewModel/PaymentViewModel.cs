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
using System.Windows.Controls;
using System.Windows.Input;

namespace coffeeX.ViewModel
{
    class PaymentViewModel : BaseViewModel
    {
        private Ingredient _currentIngredient;
        public Ingredient currentIngredient
        {
            get => _currentIngredient; set
            {
                _currentIngredient = value;
                OnPropertyChanged();
            }
        }

        private int _currentQuantity;
        public int currentQuantity { get => _currentQuantity; set
            {
                _currentQuantity = value;
                OnPropertyChanged();
            } }


        private PaymentDetail _currentDetail;
        public PaymentDetail currentDetail
        {
            get => _currentDetail; set
            {
                _currentDetail = value;
                OnPropertyChanged();
            }

        }


        public PaymentViewModel()
        {

            initCmd();
            initVariable();
            initWorker();
            loadData();
        }

        private void initCmd()
        {
            onLoaded = new RelayCommand<PaymentWindow>((p) => p != null,onWindowLoaded);
             
        }

  
        private void initVariable()
        {
            _ingredients = new ObservableCollection<Ingredient>();
        }

 

        private ObservableCollection<Ingredient> _ingredients;
        public ObservableCollection<Ingredient> ingredients { get => _ingredients; set
            {
                _ingredients = value;
                OnPropertyChanged();
            } }
        private void loadData()
        {
            loadIngredientWorker.RunWorkerAsync();
        }

        private void initWorker()
        {
            loadIngredientWorker.DoWork += LoadIngredientWorker_DoWork;
            loadIngredientWorker.RunWorkerCompleted += LoadIngredientWorker_RunWorkerCompleted;
        }

        private void LoadIngredientWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            (e.Result as List<Ingredient>).ForEach(ingredients.Add);
        }

        private void LoadIngredientWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<Ingredient> ingredients = CoffeeXRepo.Ins.DB.Ingredients.ToList();
            e.Result = ingredients;
        }

        private void onWindowLoaded(PaymentWindow obj)
        {
            obj.priceTextBox.PreviewTextInput += numberOnlyPreviewTextInput;
            obj.quantityTextBox.PreviewTextInput += numberOnlyPreviewTextInput;
            obj.ingredientComboBox.SelectionChanged += IngredientComboBox_SelectionChanged;
        }

        private void IngredientComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (currentDetail == null)
                currentDetail = new PaymentDetail();
        }

        private void numberOnlyPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        public ICommand onLoaded { get; set; }
        private BackgroundWorker loadIngredientWorker=new BackgroundWorker();

    }
}
