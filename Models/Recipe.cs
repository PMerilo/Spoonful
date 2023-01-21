using System.ComponentModel.DataAnnotations;

namespace Spoonful.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string? name { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string? description { get; set; }

        [Required]
        [Display(Name = "Preparation Time (Minutes)")]
        public double? prepTime { get; set; }

        [Required]
        [Display(Name = "Allegrens")]
        public string? allergens { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string? category { get; set; } = "Undefined";


    }
}
