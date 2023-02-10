using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spoonful.Models
{
    public class Notification
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string? Title { get; set; }

        [Required]
        [Display(Name = "Body")]
        public string? Body { get; set; }

        [Required]
        [Display(Name = "Url")]
        [DataType(DataType.Url)]
        public string? Url { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.UtcNow;

        [Required]
        public bool Seen { get; set; } = false;
        
        [Required]
        [ForeignKey("UserId")]
        public CustomerUser User { get; set; }

    }
}
