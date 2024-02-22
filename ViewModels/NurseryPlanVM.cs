namespace Nursery.ViewModels
{
    public class NurseryPlanVM
    {
        
        public string PlanTitleAr { get; set; }
        public string PlanTitleEn { get; set; }
        public bool IsActive { get; set; }
        public double Price { get; set; }
        public int NurseryPlanTypeId { get; set; }
        public int NurseryMemberId { get; set; }
    }
}
