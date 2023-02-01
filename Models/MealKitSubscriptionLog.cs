using System.ComponentModel.DataAnnotations;

namespace Spoonful.Models
{
    public class MealKitSubscriptionLog
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "No of  Users Subscribed")]
        public int? noOfUsersSubscribed { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string? description { get; set; }
    }
}
