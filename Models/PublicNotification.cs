using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Nursery.Models
{
    public class PublicNotification
    {

        public PublicNotification()
        {
            PublicNotificationDevice = new HashSet<PublicNotificationDevice>();
        }

        public int PublicNotificationId { get; set; }       
  
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
        //public int? EntityTypeId { get; set; }
        public int? EntityTypeNotifyId { get; set; }
        public int? EntityId { get; set; }
        [NotMapped]
        public string EntityNameAr { get; set; }

        [NotMapped]
        public string EntityNameEn { get; set; }
        public int CountryId { get; set; }

        public virtual Country Country { get; set; }
        // public virtual EntityType EntityType { get; set; }
        public virtual EntityTypeNotify EntityTypeNotify { get; set; }
        public virtual ICollection<PublicNotificationDevice> PublicNotificationDevice { get; set; }
        



    }
}
