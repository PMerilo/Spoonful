namespace Spoonful.Models
{
	public class Followers
	{
		public int Id { get; set; }

		public string FollowerId { get; set; }

		public virtual CustomerUser Follower { get; set; }

		public string FollowingId { get; set; }

		public virtual CustomerUser Following { get; set; }
	}
}
