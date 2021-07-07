using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using coffeeX.Model;
using coffeeX.Repository;
using coffeeX.View;

namespace coffeeX.ViewModel
{
    public class StatisticViewModel : BaseViewModel
    {
        public ICommand onLoadedCmd { get; set; }
        public ICommand onBackCmd { get; set; }
        public ICommand calcStatisticCmd { get; set; }


        private ObservableCollection<ReceiptDetail> _receiptStatistic;
        public ObservableCollection<ReceiptDetail> receiptStatistic
        {
            get => _receiptStatistic; set
            {
                _receiptStatistic = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<PaymentDetail> _paymentStatistic;
        public ObservableCollection<PaymentDetail> paymentStatistic
        {
            get => _paymentStatistic; set
            {
                _paymentStatistic = value;
                OnPropertyChanged();
            }
        }

        private double _totalEarn;
        public double totalEarn
        {
            get => _totalEarn; set
            {
                _totalEarn = value;
                OnPropertyChanged();
            }
        }
        private double _totalPay;
        public double totalPay
        {
            get => _totalPay; set
            {
                _totalPay = value;
                OnPropertyChanged();
            }
        }
        private double _revenue;
        public double revenue
        {
            get => _revenue; set
            {
                _revenue = value;
                OnPropertyChanged();
            }
        }
        public StatisticViewModel()
        {

            initCmd();
            initVariable();
        }

        private void initVariable()
        {
            _paymentStatistic = new ObservableCollection<PaymentDetail>();
            _receiptStatistic = new ObservableCollection<ReceiptDetail>();
        }

        private void initCmd()
        {
            onLoadedCmd = new RelayCommand<StatisticWindow>((p) => p != null, onWindowLoaded);
            calcStatisticCmd = new RelayCommand<StatisticWindow>((p) => p != null, calcStatistic);
            onBackCmd = new RelayCommand<StatisticWindow>((p) => p != null, (p) => { 
                p.Close(); 
                receiptStatistic.Clear(); 
                paymentStatistic.Clear();
            });
        }

        private void calcStatistic(StatisticWindow statisticWD)
        {
            receiptStatistic.Clear();
            paymentStatistic.Clear();
            int day = 0, month = 0, year = 0;
            if (statisticWD.cmbDay.SelectedItem != null)
                int.TryParse(statisticWD.cmbDay.Text, out day);
            if (statisticWD.cmbMonth.SelectedItem != null)
                int.TryParse(statisticWD.cmbMonth.Text, out month);
            if (statisticWD.cmbYear.SelectedItem != null)
                int.TryParse(statisticWD.cmbYear.Text, out year);
            List<Receipt> receipts = null;
            List<PaymentVoucher> paymentVouchers = null;
            if (day != 0 && month != 0 && year != 0)
            {
                receipts = CoffeeXRepo.Ins.DB.Receipts.Where((r) => r.dateCreated.Day == day && r.dateCreated.Month == month && r.dateCreated.Year == year).ToList();
                paymentVouchers = CoffeeXRepo.Ins.DB.PaymentVouchers.Where((r) => r.dateCreated.Day == day && r.dateCreated.Month == month && r.dateCreated.Year == year).ToList();
            }
            else if (month != 0 && year != 0)
            {
                receipts = CoffeeXRepo.Ins.DB.Receipts.Where((r) => r.dateCreated.Month == month && r.dateCreated.Year == year).ToList();
                paymentVouchers = CoffeeXRepo.Ins.DB.PaymentVouchers.Where((r) => r.dateCreated.Month == month && r.dateCreated.Year == year).ToList();
            }
            else if (year != 0)
            {
                receipts = CoffeeXRepo.Ins.DB.Receipts.Where((r) => r.dateCreated.Year == year).ToList();
                paymentVouchers = CoffeeXRepo.Ins.DB.PaymentVouchers.Where((r) => r.dateCreated.Year == year).ToList();
            }
            if (receipts != null && paymentVouchers != null)
            {
                receipts.ForEach((it) =>
                {

                    it.ReceiptDetails.ToList().ForEach((dt) =>
                    {
                        dt.total = dt.quantity * dt.Beverage.beveragePrice;
                        receiptStatistic.Add(dt);
                    });
                });
                totalEarn = receiptStatistic.Sum(it => it.total);
                paymentVouchers.ForEach((it) =>
                {

                    it.PaymentDetails.ToList().ForEach((dt) =>
                    {
                        dt.total = dt.ingredientQuantity * dt.Ingredient.ingredientPrice;
                        paymentStatistic.Add(dt);
                    });
                });
                totalPay = paymentStatistic.Sum(it => it.total);
                revenue = totalEarn - totalPay;
            }


        }

        private void onWindowLoaded(StatisticWindow obj)
        {

        }
    }
}
