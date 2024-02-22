
using System;
using System.Collections.Generic;

namespace Nursery.Models
{
    public partial class AgeCategory
    {
        public AgeCategory()
        {
            NurseryMember = new HashSet<NurseryMember>();
        }

        public int AgeCategoryId { get; set; }
        public string AgeCategoryTlAr { get; set; }
        public string AgeCategoryTlEn { get; set; }
        public int from { get; set; }
        public int to { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool IsActive { get; set; }
        public virtual ICollection<NurseryMember> NurseryMember { get; set; }
    }
}