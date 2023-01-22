using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Spoonful.Models
{
    public class CustomerUser: IdentityUser
    {
        [PersonalData]
        public string? FirstName { get; set; }

        [PersonalData]
        public string? LastName { get; set; }

        [PersonalData]
        public DateTime DOB { get; set; }

        [MaxLength(50)]
        public string? ImageURL { get; set; }

        public ICollection<Notification>? Notifications { get; set; }
    }
}
