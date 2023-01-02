using System.ComponentModel.DataAnnotations;

namespace Spoonful.Models
{
    public class MenuItem
    {
        
        public int Id { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public string? Tags { get; set; }

        [Required]
        [Display(Name = "Menu Preferences")]
        public string? MenuPreference { get; set; }
        [Required]
        public string? Category { get; set; }

        [MaxLength(50)]
        public string ImageURL { get; set; } = "/uploads/user.png";
    }
}
