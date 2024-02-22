namespace Nursery.ViewModels
{
    public class EditNurseryPlanVM
    {

        public int NurseryPlanId { get; set; }
        public string PlanTitleAr { get; set; }
        public string PlanTitleEn { get; set; }
        public bool IsActive { get; set; }
        public double Price { get; set; }
        public int NurseryPlanTypeId { get; set; }
    }
}
