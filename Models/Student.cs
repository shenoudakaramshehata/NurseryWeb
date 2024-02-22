using System;
using System.Collections.Generic;

namespace Nursery.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Pic { get; set; }
        public int Age { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int? NurseryPlanId { get; set; }
        public virtual NurseryPlan NurseryPlan { get; set; }
        public int NurseryMemberId { get; set; }
        public virtual NurseryMember NurseryMember { get; set; }
        public virtual ICollection<StudentAttachment> StudentAttachments { get; set; }
        public virtual ICollection<StudentGroup> StudentGroups { get; set; }
    }
}
