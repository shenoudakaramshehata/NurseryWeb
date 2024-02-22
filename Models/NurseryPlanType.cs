using System.Collections.Generic;

namespace Nursery.Models
{
    public class NurseryPlanType
    {
        public int NurseryPlanTypeId { get; set; }
        public string NurseryPlanTypeTitleAr { get; set; }
        public string NurseryPlanTypeTitleEn { get; set; }
        public virtual ICollection<NurseryPlan> NurseryPlans { get; set; }
    }
}
