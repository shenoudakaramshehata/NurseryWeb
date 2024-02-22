using System;

namespace Nursery.Models
{
    public class StudentGroup
    {
        public int StudentGroupId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? StudentId { get; set; }
        public virtual Student Student { get; set; }
        public int? GroupId { get; set; }
        public virtual Group Group { get; set; }
    }
}
