using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spoonful.Models
{
    public class Messages
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Message")]
        public string? Message { get; set; }

        [Required]
        public string? SenderId { get; set; }

        [Required]
        public CustomerUser Sender { get; set; }

        [Required]
        public string? ReceiverId { get; set; }

        [Required]
        public CustomerUser Receiver { get; set; }


    }
}
