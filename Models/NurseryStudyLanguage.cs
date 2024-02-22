using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Nursery.Models
{
    public class NurseryStudyLanguage
    {
        [Key]
        public int NurseryStudyLanguageId { get; set; }
        public int NurseryId { get; set; }
        [JsonIgnore]
        public virtual NurseryMember Nursery { get; set; }
        public int LanguageId { get; set; }
        [JsonIgnore]
        public virtual Language Language { get; set; }

    }
}
