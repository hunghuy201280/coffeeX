using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using coffeeX.View;

namespace coffeeX.ViewModel
{
    class StatisticViewModel : BaseViewModel
    {
        public ICommand onLoadedCmd;
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
        }

        private void initCmd()
        {
            onLoadedCmd = new RelayCommand<StatisticWindow>((p) => p != null, onWindowLoaded);
        }

        private void onWindowLoaded(StatisticWindow obj)
        {
            throw new NotImplementedException();
        }
    }
}
