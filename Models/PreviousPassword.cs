using Spoonful.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace Spoonful.Models
{
	public class PreviousPassword
	{
		public int Id { get; set; }

		[Required]
		public string Password { get; set; }

		[Required]
		public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.UtcNow;

		[Required]
		public string CustomerUserId { get; set; }

		[Required]
		public CustomerUser CustomerUser { get; set; }
	}
}
