using coffeeX.Model;
using coffeeX.Repository;
using coffeeX.Utils;
using coffeeX.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace coffeeX.ViewModel
{
    class PaymentViewModel : BaseViewModel
    {
  


        private ObservableCollection<PaymentDetail> _paymentDetails;
        public ObservableCollection<PaymentDetail> paymentDetails
        {
            get => _paymentDetails; set
            {
                _paymentDetails = value;
                OnPropertyChanged();
            }

        }
        private double _paymentValue;
        public double paymentValue
        {
            get => _paymentValue; set
            {
                _paymentValue = value;
                OnPropertyChanged();
            }

        }

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
            
            confirmCmd = new RelayCommand<Object>((p) => paymentDetails!=null && paymentDetails.Count>0, onConfirmClick);
            onLoaded = new RelayCommand<PaymentWindow>((p) => p != null, onWindowLoaded);
            cancelCmd = new RelayCommand<PaymentWindow>((p) => p != null, resetData);
            increaseQuantity = new RelayCommand<DataGrid>((p) => true, (p) =>
            {
                (p.SelectedItem as PaymentDetail).ingredientQuantity++;
                new Thread(() =>
                {
                    paymentValue = paymentDetails.Sum(it => it.total);
                }).Start();
            });
            decreaseQuantity = new RelayCommand<DataGrid>((p) => true, (p) =>
            {
                if ((p.SelectedItem as PaymentDetail).ingredientQuantity > 1)
                    (p.SelectedItem as PaymentDetail).ingredientQuantity--;
                new Thread(() =>
                {
                    paymentValue = paymentDetails.Sum(it => it.total);

                }).Start();
            });
            deletePaymentItemCmd = new RelayCommand<DataGrid>((p) => true, (p) =>
            {
                paymentDetails.Remove(p.SelectedItem as PaymentDetail);
                new Thread(() =>
                {
                    paymentValue = paymentDetails.Sum(it => it.total);

                }).Start();
            });
            addPaymentItemCmd = new RelayCommand<PaymentWindow>((p) => true, addToPaymentVoucher);
        }

        private void onConfirmClick(object obj)
        {
          
          


            List<PaymentDetail> details = new List<PaymentDetail>();
            for (int i = 0; i < _paymentDetails.Count; i++)
            {
                var tempDetail = _paymentDetails[i];
                var tempIngredient = _ingredients.Where(ing => ing.ingredientName ==tempDetail.Ingredient.ingredientName).ToList();
                int ingredientId = -1;
                if(tempIngredient.Count>0)
                {
                    ingredientId = tempIngredient.First().ingredientID;
                    details.Add(new PaymentDetail()
                    {
                        ingredientID=ingredientId,
                        ingredientQuantity= tempDetail.ingredientQuantity
                    });
                }
               
            }
            int currentUserID = (wd.staffTextBlock.DataContext as UserViewModel).currentUser.userID;

            PaymentVoucher paymenVoucher = new PaymentVoucher()
            {
                paymentValue = _paymentValue,
                userID = currentUserID,
                dateCreated = DateTime.Now,
                PaymentDetails = details,
            };
            CoffeeXRepo.Ins.DB.PaymentVouchers.Add(paymenVoucher);

            CoffeeXRepo.Ins.DB.SaveChanges();
            new NotifyPwdWindow("Thêm phiếu chi thành công!").Show();

            resetData(wd);
        }

        private void resetData(PaymentWindow paymentWD)
        {
            _paymentDetails.Clear();
            _paymentValue = 0;
            _currentDetail = new PaymentDetail();
            _currentDetail.Ingredient = new Ingredient();
            paymentWD.Close();
            
        }

        private void addToPaymentVoucher(PaymentWindow p)
        {
            if (String.IsNullOrEmpty(p.priceTextBox.Text)||
                String.IsNullOrEmpty(p.ingredientComboBox.Text)
                || String.IsNullOrEmpty(p.unitNameTextBox.Text)
                || String.IsNullOrEmpty(p.quantityTextBox.Text)
                )
            {
                new NotifyPwdWindow("Vui lòng nhập đầy đủ thông tin!").ShowDialog();
                return;
            }
            if(int.Parse(p.quantityTextBox.Text)<=0)
            {
                new NotifyPwdWindow("Vui lòng nhập số lượng !").ShowDialog();
                return;
            }
            if (currentDetail.Ingredient.ingredientName != null)
            {
                if (paymentDetails.Any(it => it.Ingredient.ingredientName == currentDetail.Ingredient.ingredientName))
                {

                    paymentDetails.Where(it => it.Ingredient.ingredientName == currentDetail.Ingredient.ingredientName).ToList()[0].ingredientQuantity += currentDetail.ingredientQuantity;
                }
                else
                {
                    paymentDetails.Add(new PaymentDetail()
                    {
                        Ingredient = currentDetail.Ingredient,
                        ingredientQuantity = currentDetail.ingredientQuantity
                    });
                }
            }
          /*  else
            {

                int quantity = int.Parse(p.quantityTextBox.Text);
                Unit unit = new Unit()
                {
                    unitName = p.unitNameTextBox.Text
                };
                Ingredient ingredient = new Ingredient()
                {
                    Unit = unit,
                    ingredientName = p.ingredientComboBox.Text,
                    ingredientPrice = Double.Parse(p.priceTextBox.Text),
             
                };
                if (paymentDetails.Any(it => it.Ingredient.ingredientName == ingredient.ingredientName))
                {

                    paymentDetails.Where(it => it.Ingredient.ingredientName == ingredient.ingredientName).ToList()[0].ingredientQuantity += quantity;
                }
                else
                {
                    paymentDetails.Add(new PaymentDetail()
                    {
                        Ingredient = ingredient,
                        ingredientQuantity = quantity
                    });
                }
           
            }*/
            paymentValue = paymentDetails.Sum(it => it.total);



        }
        private void initVariable()
        {
            _ingredients = new ObservableCollection<Ingredient>();
            _units = new ObservableCollection<Unit>();
        }


      
        public void loadData()
        {
            loadIngredientWorker.RunWorkerAsync();
            loadUnitWorker.RunWorkerAsync();
        }

        private void initWorker()
        {
            loadIngredientWorker.DoWork += LoadIngredientWorker_DoWork;
            loadIngredientWorker.RunWorkerCompleted += LoadIngredientWorker_RunWorkerCompleted;
            loadUnitWorker.DoWork += LoadUnitWorker_DoWork;
            loadUnitWorker.RunWorkerCompleted += LoadUnitWorker_RunWorkerCompleted;
        }

        private void LoadUnitWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            (e.Result as List<Unit>).ForEach(units.Add);

        }

        private void LoadUnitWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<Unit> result;
            using (CoffeeXEntities db=new CoffeeXEntities())
            {
                 result = db.Units.ToList();
            }
            e.Result = result;


        }

        private void LoadIngredientWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            (e.Result as List<Ingredient>).ForEach(ingredients.Add);
        }

        private void LoadIngredientWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<Ingredient> ingredients;
           
                 ingredients = CoffeeXRepo.Ins.DB.Ingredients.ToList();
            
            e.Result = ingredients;
        }

        private void onWindowLoaded(PaymentWindow obj)
        {
            obj.priceTextBox.PreviewTextInput += numberOnlyPreviewTextInput;
            obj.quantityTextBox.PreviewTextInput += numberOnlyPreviewTextInput;
            obj.ingredientComboBox.SelectionChanged += IngredientComboBox_SelectionChanged;
            currentDetail = new PaymentDetail();
            currentDetail.Ingredient = new Ingredient();
            paymentDetails = new ObservableCollection<PaymentDetail>();
            wd = obj;
        }

        private void IngredientComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          /*  ComboBox comboBox = sender as ComboBox;
            if (comboBox.SelectedItem != null)
            {
                Ingredient temp = comboBox.SelectedItem as Ingredient;
                if (temp.ingredientName != null)
                {
                    wd.unitNameTextBox.IsEnabled = false;
                    wd.priceTextBox.IsEnabled = false;
                }
            }
            else
            {
                currentDetail.Ingredient = new Ingredient();
                wd.unitNameTextBox.IsEnabled = true;
                wd.priceTextBox.IsEnabled = true;
            }*/
        }

        private void numberOnlyPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        public ICommand onLoaded { get; set; }
        private BackgroundWorker loadIngredientWorker = new BackgroundWorker();
        private BackgroundWorker loadUnitWorker = new BackgroundWorker();
        public ICommand increaseQuantity { get; set; }
        public ICommand decreaseQuantity { get; set; }
        public ICommand deletePaymentItemCmd { get; set; }
        public ICommand addPaymentItemCmd { get; set; }
        public ICommand cancelCmd { get; set; }
        public ICommand confirmCmd { get; set; }
        private PaymentWindow wd;
        private ObservableCollection<Ingredient> _ingredients;
        public ObservableCollection<Ingredient> ingredients
        {
            get => _ingredients; set
            {
                _ingredients = value;
                OnPropertyChanged();
            }
        } 
        private ObservableCollection<Unit> _units;
        public ObservableCollection<Unit> units
        {
            get => _units; set
            {
                _units = value;
                OnPropertyChanged();
            }
        }
    }
}
