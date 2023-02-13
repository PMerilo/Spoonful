using System.ComponentModel.DataAnnotations;

namespace Spoonful.Models
{
    public class OrderDetails
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Required]
        [Display(Name = "Order Date")]
        public string? OrderDate { get; set; }

        
        [Display(Name = "Delivery Date")]
        public string? DeliveryDate { get; set; }

        [Required]
        [Display(Name = "Order Time")]
        public string? OrderTime { get; set; }

        
        [Display(Name = "Additional Instructions")]
        public string? AdditionalInstructions { get; set; }

        [Required]
        public string? userId { get; set; }

        public Boolean SubscriptionCheck { get; set; } = false;


    }
}
