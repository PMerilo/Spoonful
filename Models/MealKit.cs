using System.ComponentModel.DataAnnotations;

namespace Spoonful.Models
{
    public class MealKit
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Menu Preferences")]
        public string? MenuPreference { get; set; }

        [Required]
        [Display(Name = "Number Of Recipes Per Week")]
        public string? noOfRecipesPerWeek { get; set; }

        [Required]
        [Display(Name = "Number Of People Per Week")]
        public string? noOfPeoplePerWeek { get; set; }

        [Required]
        [Display(Name = "Number Of Servings Per Person")]
        public string? noOfServingsPerPerson { get; set; }

        [Required]
        public string? userId { get; set; }

        public Boolean Status { get; set; } = true;




    }
}
