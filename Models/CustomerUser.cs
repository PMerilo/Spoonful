using Microsoft.AspNetCore.Identity;

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
    }
}
