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
            CustomerDetails = _db.CustomerDetails.Include(d => d.User).ToList();


        }

        public async Task<IActionResult> OnPostAddFriend(string username)
        {
            var notification = new Notification()
            {
                Title = "New Friend Request",
                Body = $"@{User.Identity.Name} has requested to be your friend",
                Url = "/User/Find",
            };
            await _notificationService.SendNotificationAsync(notification, username);
            return RedirectToPage();
        }
    }
}
