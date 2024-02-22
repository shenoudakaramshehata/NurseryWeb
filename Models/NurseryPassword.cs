using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Nursery.Models
{
    public class NurseryPassword
    {
        [Key]
        public int NurseryPasswordId { get; set; }
        [Required]
        public string Password { get; set; }
        public int NurseryMemberId { get; set; }
        [JsonIgnore]
        public virtual NurseryMember NurseryMember { get; set; }

    }
}
