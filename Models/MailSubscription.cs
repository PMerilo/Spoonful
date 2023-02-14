using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Spoonful.Models
{
    public class MailSubscription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string email { get; set; }

        [Required]
        public string Subscriptiontype { get; set; }

        [Required]
        public string datetime { get; set; } = DateTime.Now.ToShortDateString().ToString();
    }
}
