using System.Collections.Generic;

namespace Nursery.Models
{
    public class NurseryPlan
    {
        public int NurseryPlanId { get; set; }
        public string PlanTitleAr { get; set; }
        public string PlanTitleEn { get; set; }
        public bool IsActive { get; set; }
        public double Price { get; set; }
        public int NurseryPlanTypeId { get; set; }
        public virtual NurseryPlanType NurseryPlanType { get; set; }
        public int NurseryMemberId { get; set; }
        public virtual NurseryMember NurseryMember { get; set; }
       // public virtual ICollection<Student> Students { get; set; }
    }
}
