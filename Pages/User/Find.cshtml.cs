using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using sib_api_v3_sdk.Model;
using Spoonful.Models;
using Spoonful.Services;
using Task = System.Threading.Tasks.Task;

namespace Spoonful.Pages.User
{
    [Authorize]
    public class FindModel : PageModel
    {
        private readonly AuthDbContext _db;
        private readonly NotificationService _notificationService;

        public FindModel(AuthDbContext db, NotificationService notificationService)
        {
            _db = db;
            _notificationService = notificationService;
        }
        public List<CustomerDetails> CustomerDetails { get; set; }

        public void OnGet()
        {
			var user = _db.Users.Include(u => u.Followings).Include(u => u.Followers).FirstOrDefault(u => u.UserName == User.Identity.Name);
            CustomerDetails = _db.CustomerDetails.Include(d => d.User).Where(u => u.UserId != user.Id).ToList();
            Console.WriteLine("Followers: "+ user.Followers.Count);
            Console.WriteLine("Following: " + user.Followings.Count);

		}

		public async Task<IActionResult> OnPostAddFriend(string username)
        {
            var user =_db.Users.Include(u => u.Followings).FirstOrDefault(u => u.UserName == User.Identity.Name);
            var targetuser = _db.Users.FirstOrDefault(u => u.UserName == username);
            user.Followings.Add(new Followers
            {
                Follower = user,
                Following = targetuser
			});
            _db.SaveChanges();
            return RedirectToPage();
        }
    }
}
