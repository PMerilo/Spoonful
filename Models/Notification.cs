using System.ComponentModel.DataAnnotations;

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
        [DataType(DataType.DateTime)]
        public string? DateCreated { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public CustomerUser User { get; set; }

    }
}
