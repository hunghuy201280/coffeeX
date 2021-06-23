using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for StaticWindow.xaml
    /// </summary>

    
    public partial class StaticWindow : Window
    {



        

        List<int> year;
        List<int> month = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        
        public StaticWindow()
        {
            //temp set date time
            DateTime time = DateTime.Today;

            

            InitializeComponent();
            
            year = Enumerable.Range(2020, 11).ToList();
            cmbYear.ItemsSource = year;
            cmbYear.SelectedItem = time.Year;

            cmbMonth.ItemsSource = month;
            cmbMonth.SelectedItem = DateTime.Today.Month;

            resetDayInMonth();
            cmbDay.SelectedItem = DateTime.Today.Day;


            cmbMonth.SelectionChanged += CmbMonth_SelectionChanged;
            cmbYear.SelectionChanged += CmbYear_SelectionChanged;

        }

        private void CmbYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            resetDayInMonth();
        }

        private void CmbMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            resetDayInMonth();
        }

        void resetDayInMonth()
        {
            List<int> day;

            int tempyear = Convert.ToInt32(cmbYear.SelectedValue);
            int teammonth = Convert.ToInt32(cmbMonth.SelectedValue);

            day = Enumerable.Range(1, DateTime.DaysInMonth(tempyear, teammonth)).ToList();

            cmbDay.ItemsSource = day;
        }
    }

}
