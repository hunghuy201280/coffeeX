using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace coffeeX.ViewModel
{
  public  class NotifyViewModel :BaseViewModel
    {
        public ICommand buttonCommand { get; set; }



        public NotifyViewModel()
        {
            buttonCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { p.Close(); });
        }


      
    }

}
