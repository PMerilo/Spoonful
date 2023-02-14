using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.User
{
    public class IndexModel : PageModel
    {
        private readonly AuthDbContext _db;
        private readonly NotificationService _notificationService;
        private readonly INotyfService _toastService;
        private readonly MessagingService messagingService;

        public IndexModel(AuthDbContext db, NotificationService notificationService, INotyfService toastService, MessagingService messagingService)
        {
            _db = db;
            _notificationService = notificationService;
            _toastService = toastService;
            this.messagingService = messagingService;
        }
        public CustomerDetails TargetUser { get; set; }

        public async Task<IActionResult> OnGetAsync(string Username)
        {
            TargetUser = _db.CustomerDetails.Include(d => d.User).ThenInclude(d => d.Followers).Include(d => d.User).ThenInclude(d => d.Followings).FirstOrDefault(u => u.User.UserName == Username);
            if (TargetUser == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
