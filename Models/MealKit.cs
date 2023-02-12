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
        public int? noOfRecipesPerWeek { get; set; }

        [Required]
        [Display(Name = "Number Of People Per Week")]
        public int? noOfPeoplePerWeek { get; set; }

        [Required]
        [Display(Name = "Number Of Servings Per Person")]
        public int? noOfServingsPerPerson { get; set; }

        [Required]
        public string? userId { get; set; }

        [Required]
        public string? Name{ get; set; }

        [Required]
        public string? Email { get; set; }


        public string? orderDetailsId { get; set; }

        public Boolean Status { get; set; } = true;

        public Boolean SubscriptionCheck { get; set; } = false;




    }
}
