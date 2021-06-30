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
    /// Interaction logic for ChoiceBeverageManageDialog.xaml
    /// </summary>
    public partial class ChoiceBeverageManageDialog : Window
    {
        public ChoiceBeverageManageDialog()
        {
            InitializeComponent();
        }

        

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void modifyBeverageButton_Click(object sender, RoutedEventArgs e)
        {
            new UpdateBeverageWindow().Show();
            this.Close();
        }

        private void addBeverageButton_Click(object sender, RoutedEventArgs e)
        {
            new AddBeverageWindow().Show();
            this.Close();
        }
    }
}
