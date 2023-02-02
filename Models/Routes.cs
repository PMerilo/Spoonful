using System.ComponentModel.DataAnnotations;

namespace Spoonful.Models
{
    public class Routes
    {
        public int Id { get; set; }

        [Required]
        public string? Region { get; set; }

        [Required]
        public string? Town { get; set; }

        public ICollection<CustomerUser> CustomerUser { get; set; }

        public ICollection<Stops> Stops { get; set; }
    }   
}

// ? is nullable and [required] if to make sure sql is not nullable but c# is nullable
