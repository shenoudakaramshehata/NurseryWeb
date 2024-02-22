using System;

namespace Nursery.Models
{
    public class AcademyYear
    {
        public int AcademyYearId { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public int NurseryMemberId { get; set; }
        public virtual NurseryMember NurseryMember { get; set; }


    }
}
