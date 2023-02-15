using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using sib_api_v3_sdk.Model;
using Spoonful.Models;
using Spoonful.Services;
using Task = System.Threading.Tasks.Task;

namespace Spoonful.Pages
{
    [Authorize]
    public class UsersModel : PageModel
    {
        private readonly AuthDbContext _db;
        private readonly NotificationService _notificationService;
        private readonly INotyfService _toastService;

        public UsersModel(AuthDbContext db, NotificationService notificationService, INotyfService toastService)
        {
            _db = db;
            _notificationService = notificationService;
            _toastService = toastService;
        }
        public List<CustomerDetails> CustomerDetails { get; set; }
        public CustomerUser CurrentUser { get; set; }
        [BindProperty]
        public string Action { get; set; }
        [BindProperty]
        public string TargetUser { get; set; }

        public void OnGet()
        {
            var user = _db.Users.Include(u => u.Followings).Include(u => u.Followers).FirstOrDefault(u => u.UserName == User.Identity.Name);
            CurrentUser = user;
            CustomerDetails = _db.CustomerDetails.Include(d => d.User).ThenInclude(d => d.Followers).Include(d => d.User).ThenInclude(d => d.Followings).Where(u => u.UserId != user.Id).ToList();

        }

        private async Task<IActionResult> Follow(string username)
        {
            var user = _db.Users.Include(u => u.Followings).FirstOrDefault(u => u.UserName == User.Identity.Name);
            var targetuser = _db.Users.FirstOrDefault(u => u.UserName == username);
            var following = new Followers
            {
                Follower = user,
                Following = targetuser
            };
            var contains = user.Followings.FirstOrDefault(f => f.Follower == user && f.Following == targetuser);
            if (contains != null)
            {
                _toastService.Error($"You already follow this user");
                return RedirectToPage();
            }
            user.Followings.Add(following);
            _toastService.Success($"Now Following {targetuser.UserName}");
            var notification = new Models.Notification
            {
                Title = "New follower",
                Body = $"@{user.UserName} has started following you.",
                Url = $"/User/{user.UserName}"
            };
            await _notificationService.SendNotificationAsync(notification, targetuser.UserName);
            _db.SaveChanges();
            return RedirectToPage();
        }
        private async Task<IActionResult> Unfollow(string username)
        {
            var user = _db.Users.Include(u => u.Followings).FirstOrDefault(u => u.UserName == User.Identity.Name);
            var targetuser = _db.Users.FirstOrDefault(u => u.UserName == username);
            var contains = user.Followings.FirstOrDefault(f => f.Follower == user && f.Following == targetuser);
            if (contains == null)
            {
                _toastService.Error($"You do not follow this user");
                return RedirectToPage();
            }
            user.Followings.Remove(contains);
            _toastService.Success($"You have unfollowed @{targetuser.UserName}");
            _db.SaveChanges();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var username = TargetUser;
            if (Action == "Follow")
            {
                return await Follow(username);
            } else
            {
                return await Unfollow(username);

            }
        }
    }
}
