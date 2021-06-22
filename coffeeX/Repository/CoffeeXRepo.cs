using coffeeX.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffeeX.Repository
{
    class CoffeeXRepo
    {
        private static CoffeeXRepo _ins;
        public static CoffeeXRepo Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new CoffeeXRepo();
                return _ins;
            }
            set
            {
                _ins = value;
            }
        }

        public CoffeeXEntities DB { get; set; }

        private CoffeeXRepo()
        {
            DB = new CoffeeXEntities();
        }
    }
}
