using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spoonful.Models
{
    public class UserDetails
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public CustomerUser User { get; set; } 

        public string UserType { get; set; }
    }

    public class AdminDetails : UserDetails
    {
        public string? EmployeeId { get; set; }

    }

    public class CustomerDetails : UserDetails
    {
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? DietaryRestrictions { get; set; }
    }

    public class DriverDetails : UserDetails
    {
        public double Commision { get; set; }
        public double Rating { get; set; }
        public double HourlyRate { get; set; }

    }
}
