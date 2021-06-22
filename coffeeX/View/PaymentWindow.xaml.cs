using coffeeX.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace coffeeX.View
{
    /// <summary>
    /// Interaction logic for PaymentWindow.xaml
    /// </summary>
    public partial class PaymentWindow : Window
    {

        PaymentVoucher voucher;
        public PaymentWindow()
        {
            InitializeComponent();
            voucher = new PaymentVoucher();
            List<TestData> a = new List<TestData>();
            a.Add(new TestData() { num = 2, ten = "Ten san pham", gia = 30000 });
            a.Add(new TestData() { num = 2, ten = "Ten san pham", gia = 30000 });
            a.Add(new TestData() { num = 2, ten = "Ten san pham", gia = 30000 });
            paymentDataGrid.ItemsSource = a;
            
        }

        public class TestData
        {
            public int num { get; set; }
            public string ten { get; set; }
            public float gia{ get; set; }

        }
    }
}
