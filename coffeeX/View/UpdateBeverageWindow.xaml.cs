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
    /// Interaction logic for UpdateBeverageWindow.xaml
    /// </summary>
    /// 
   

    public partial class UpdateBeverageWindow : Window
    {
        public Model.Table _updateTable;
        public UpdateBeverageWindow()
        {
            InitializeComponent();
        }
        public UpdateBeverageWindow(Model.Table table)
        {
            InitializeComponent();
            _updateTable = table;
        }

        private void DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }
    }
}
