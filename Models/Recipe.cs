using System.ComponentModel.DataAnnotations;

namespace Spoonful.Models
{
	public class Recipe
	{
		public int Id { get; set; }

		[Required]
		public string? RecipeName { get; set; }

		[Required]
		public string? RecipeDesc { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string? category { get; set; } = "Undefined";

        [MaxLength(50)]
        public string ImageURL { get; set; } = "/uploads/user.png";

        [Required]
        [Display(Name = "Ingredients")]
        public string? ingredients { get; set; }

        [Required]
        [Display(Name = "Instructions")]
        public string? instructions { get; set; }
    }
		[Required]
		public string? TimeTaken { get; set; }
		
	}
}
