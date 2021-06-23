using coffeeX.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();

         
        }


        public class
       Drink
        {
            public string beverageName { get; set; }
            public string beveragePrice { get; set; }
            public string beverageImage { get; set; }


            public Drink(string beverageName, string beveragePrice, string beverageImage)
            {
                this.beverageName = beverageName;
                this.beveragePrice = beveragePrice;
                this.beverageImage = beverageImage;
            }
        }

        private void categoryListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //invoke command here
        }
    }
}
