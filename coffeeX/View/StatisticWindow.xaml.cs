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

    
    public partial class StatisticWindow : Window
    {

        List<int> year;
        List<int> month = new List<int>() { 0,1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        
        public StatisticWindow()
        {
            //temp set date time
            DateTime time = DateTime.Today;

            

            InitializeComponent();
            
            year = Enumerable.Range(2021, 11).ToList();
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

            int tempYear = Convert.ToInt32(cmbYear.SelectedValue);
            int tempMonth = Convert.ToInt32(cmbMonth.SelectedValue);
            if (tempMonth == 0)
            {
                day = new List<int>();
                day.Add(0);
            }
            else
            {
                day = Enumerable.Range(0, DateTime.DaysInMonth(tempYear, tempMonth) + 1).ToList();
            }
            cmbDay.ItemsSource = day;
        }
    }

}
