using System.Collections.Generic;

namespace Nursery.Models
{
    public class DocumentType
    {
        public int DocumentTypeId { get; set; }
        public string DocumentTypeAr { get; set; }
        public string DocumentTypeEn { get; set; }
        public virtual ICollection<StudentAttachment> StudentAttachments { get; set; }


    }
}
