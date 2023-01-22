using System.ComponentModel.DataAnnotations;

namespace Spoonful.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Required]
        [Display(Name = "Order Date")]
        public string? OrderDate { get; set; }

        [Required]
        [Display(Name = "Order Time")]
        public string? OrderTime { get; set; }

        [Required]
        [Display(Name = "Additional Instructions")]
        public string? AdditionalInstructions { get; set; }

        [Required]
        public string? userId { get; set; }


    }
}
