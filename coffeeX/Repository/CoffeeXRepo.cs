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
            Customer tempCus = Ins.DB.Customers.Find(customer.customerID);
            String voucherID = null;
            double receiptValue = receiptDetails.Sum(e => e.quantity * e.Beverage.beveragePrice);
            if (voucher != null)
            {
                voucherID = voucher.voucherID;
                receiptValue = receiptValue * (1 - voucher.voucherValue);
            }
            if (tempCus == null)
                tempCus = customer;
            Receipt receipt = new Receipt()
            {
                Customer = tempCus,
                UserInfo = user,
                voucherID = voucherID,
                ReceiptDetails = receiptDetails,
                dateCreated = dateCreated,
                receiptValue = receiptValue
            };
            Ins.DB.Receipts.Add(receipt);
            Ins.DB.SaveChanges();
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
