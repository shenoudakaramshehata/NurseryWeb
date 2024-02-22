
using System.ComponentModel.DataAnnotations;

namespace Nursery.Models
{
    public partial class Configuration
    {
        public int ConfigurationId { get; set; }
        [Required]
        public string Phone { get; set; }
        [EmailAddress]
        [Required, RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Not Valid")]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Facebook { get; set; }
        [Required]
        public string WhatsApp { get; set; }
        [Required]
        public string LinkedIn { get; set; }
        [Required]
        public string Instgram { get; set; }
        [Required]
        public string Twitter { get; set; }
    }
}