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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace coffeeX.Resource.UserControls
{
    /// <summary>
    /// Interaction logic for BeverageNameUC.xaml
    /// </summary>
    public partial class BeverageNameUC : UserControl
    {
        public BeverageNameUC()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public String Hint { set; get; }

    }
    
}
