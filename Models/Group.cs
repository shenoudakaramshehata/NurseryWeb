using System;
using System.Collections.Generic;

namespace Nursery.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Pic { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }
        public int NurseryMemberId { get; set; }
        public virtual NurseryMember NurseryMember { get; set; }
        public virtual ICollection<StudentGroup> StudentGroups { get; set; }

    }
}
