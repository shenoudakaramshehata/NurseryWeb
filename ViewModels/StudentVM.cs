using System;

namespace Nursery.ViewModels
{
    public class StudentVM
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int Age { get; set; }
        public DateTime Birthdate { get; set; }
        public int? NurseryPlanId { get; set; }
        public int NurseryMemberId { get; set; }
        
    }
}
