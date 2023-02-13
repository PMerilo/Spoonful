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
        public DateTime? DOB { get; set; }

        [MaxLength(50)]
        public string? ImageURL { get; set; }

        public bool isDisabled { get; set; } = false;

        public DateTimeOffset? LastLogin { get; set; }
        public DateTimeOffset LastPassChanged { get; set; } = DateTimeOffset.UtcNow;
        public bool RequirePassChange { get; set; } = false;
		public ICollection<PreviousPassword> PreviousPassword { get; set; }

		[PersonalData]
        public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.Now;

        public ICollection<Notification>? Notifications { get; set; }

		public virtual ICollection<Followers> Followers { get; set; }
		public virtual ICollection<Followers> Followings { get; set; }

		[Required]
        public virtual UserDetails UserDetails { get; set; }
    }
}
