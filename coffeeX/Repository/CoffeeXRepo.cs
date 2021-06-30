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

       static public void addReceipt(Customer customer,UserInfo user,Voucher voucher,List<ReceiptDetail> receiptDetails)
        {
            DateTime dateCreated = DateTime.Now;
            Receipt receipt = new Receipt() {
                Customer = customer,
                UserInfo = user,
                Voucher = voucher,
                ReceiptDetails=receiptDetails,
            };
            using(CoffeeXEntities db=new CoffeeXEntities())
            {
                db.Receipts.Add(receipt);
            }
        }
        static public List<Beverage> getBeverages()
        {
            using (CoffeeXEntities db = new CoffeeXEntities())
            {
                return db.Beverages.ToList();
            }
        }
    }
}
