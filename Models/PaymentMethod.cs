using System.Collections.Generic;


namespace Nursery.Models
{
    public partial class PaymentMethod
    {
        public PaymentMethod()
        {
            NurseryMember = new HashSet<NurseryMember>();
        }
        public int PaymentMethodId { get; set; }
        public string PaymentMethodTlAr { get; set; }
        public string PaymentMethodTlEn { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<NurseryMember> NurseryMember { get; set; }

    }
}