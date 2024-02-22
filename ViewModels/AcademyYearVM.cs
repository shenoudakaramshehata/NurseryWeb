using System;

namespace Nursery.ViewModels
{
    public class AcademyYearVM
    {
       
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public int NurseryMemberId { get; set; }
    }
}
