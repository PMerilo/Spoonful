using System.ComponentModel.DataAnnotations;

namespace Spoonful.Models
{
    public class Stops
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Required]
        public int RoutesId { get; set; }
        public Routes Routes { get; set; }

        public ICollection<Delivery> Delivery { get; set; }
    }   
}

// ? is nullable and [required] if to make sure sql is not nullable but c# is nullable
