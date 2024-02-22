using System;

namespace Nursery.ViewModels
{
    public class EditAcademyYearVM
    {
        public int AcademyYearId { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

    }
}
