using System.ComponentModel.DataAnnotations;

namespace Spoonful.Models
{
    public class Delivery
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Delivery Confirmation")]
        public string? deliveryConfirmation { get; set; }

        [Required]
        [Display(Name = "Customer Confirmation")]
        public string? customerConfirmation { get; set; }

        [Required]
        [Display(Name = "Delivery Date Time")]
        public string? deliveryDateTime { get; set; }

        public string? ConfirmationImageURL { get; set; }

        [Required]
        public int orderdetailsId { get; set; }
        public OrderDetails OrderDetails { get; set; }

        [Required]
        public int stopsId { get; set; }
        public Stops Stops { get; set; }


    }   
}

// ? is nullable and [required] if to make sure sql is not nullable but c# is nullable
