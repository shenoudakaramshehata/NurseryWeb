using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nursery.Models
{
  
    public partial class Parent
    {
        public Parent()
        {
            Child = new HashSet<Child>();
        }

        public int ParentId { get; set; }
        public string ParentName { get; set; }
        public string ParentAddress { get; set; }
        public string ParentPhone { get; set; }
        public string ParentEmail { get; set; }

        public virtual ICollection<Child> Child { get; set; }

    }

}
