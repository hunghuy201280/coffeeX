//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace coffeeX.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class PaymentVoucher
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PaymentVoucher()
        {
            this.PaymentDetails = new HashSet<PaymentDetail>();
        }
    
        public int paymentID { get; set; }
        public System.DateTime dateCreated { get; set; }
        public double paymentValue { get; set; }
        public int userID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaymentDetail> PaymentDetails { get; set; }
        public virtual UserInfo UserInfo { get; set; }
    }
}
