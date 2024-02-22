using System;

namespace Nursery.Models
{
    public class StudentAttachment
    {
        public int StudentAttachmentId { get; set; }
        public string FileUrl { get; set; }
        public DateTime AttachmentDate { get; set; }
        public string Description { get; set; }
        public int DocumentTypeId { get; set; }
        public virtual DocumentType DocumentType { get; set; }
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
    }
}
