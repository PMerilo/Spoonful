using System.ComponentModel.DataAnnotations;

namespace Spoonful.Models
{
    public class Recipe
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string? name { get; set; }
		public string? RecipeName { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string? description { get; set; }
		public string? RecipeDesc { get; set; }

        [Required]
        [Display(Name = "Preparation Time (Minutes)")]
        public double? prepTime { get; set; }
        [Display(Name = "Category")]
        public string? category { get; set; } = "Undefined";

        [MaxLength(50)]
        public string ImageURL { get; set; } = "/uploads/user.png";

        [Required]
        [Display(Name = "Allegrens")]
        public string? allergens { get; set; }
        [Display(Name = "Ingredients")]
        public string? ingredients { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string? category { get; set; } = "Undefined";

        [Display(Name = "Instructions")]
        public string? instructions { get; set; }
    }
		[Required]
		public string? TimeTaken { get; set; }

    }
}
