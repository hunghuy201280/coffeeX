using coffeeX.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffeeX.Model
{
    public enum TableStatus
    {
        Free, Pending, Done
    }
    public class Table : BaseViewModel
    {
        public Table(int number)
        {
            this.number = number;
            receiptItems = new ObservableCollection<ReceiptDetail>();
            status = TableStatus.Free;
            receiptValue = 0;
            currentCustomer = new Customer();
        }

        private Voucher _currentVoucher;
        public Voucher currentVoucher
        {
            get => _currentVoucher; set
            {
                _currentVoucher = value;
                calcDiscount();

                OnPropertyChanged();
            }
        }     
        private void calcDiscount()
        {
            if (currentVoucher != null)
            {
                discount = _receiptValue * _currentVoucher.voucherValue;

            }
            else
            {
                discount = 0;

            }
            total = receiptValue - discount;
        }
        
        private double _discount;
        public double discount
        {
            get => _discount; set
            {
                _discount = value;
                if (currentVoucher != null)
                {
                    _discount = _receiptValue * _currentVoucher.voucherValue;

                }
                else
                {
                    _discount = 0;

                }
                total = receiptValue - discount;


                OnPropertyChanged();
            }
        }


        private Customer _currentCustomer;
        public Customer currentCustomer
        {
            get => _currentCustomer; set
            {
                _currentCustomer = value;
                OnPropertyChanged();
            }
        }

        public void resetStatus()
        {
            receiptItems = new ObservableCollection<ReceiptDetail>();
            status = TableStatus.Free;
            receiptValue = 0;
            currentCustomer = new Customer();
            currentVoucher = null;
            discount = 0;
        }
        private int _number;

        public int number { get => _number; set { _number = value; OnPropertyChanged(); } }
        private TableStatus _status;
        public TableStatus status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<ReceiptDetail> _receiptItems;
        public ObservableCollection<ReceiptDetail> receiptItems
        {
            get => _receiptItems; set
            {

                _receiptItems = value;

                OnPropertyChanged();
            }
        }
        private double _receiptValue;
        public double receiptValue
        {
            get => _receiptValue; set
            {
                _receiptValue = value;
                calcDiscount();
                OnPropertyChanged();
            }
        }
        private double _total;
        public double total
        {
            get => _total; set
            {
                _total = value;
                OnPropertyChanged();
            }
        }
    }
}
