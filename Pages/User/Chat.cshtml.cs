using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.User
{
    public class ChatModel : PageModel
    {
        private readonly AuthDbContext _db;
        private readonly NotificationService _notificationService;
        private readonly INotyfService _toastService;

        public ChatModel(AuthDbContext db, NotificationService notificationService, INotyfService toastService)
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
        public CustomerDetails TargetUser { get; set; }
        public void OnGet(string Username)
        {
            var user = _db.Users.Include(u => u.Followings).Include(u => u.Followers).FirstOrDefault(u => u.UserName == User.Identity.Name);
            CurrentUser = user;
            CustomerDetails = _db.CustomerDetails.Include(d => d.User).ThenInclude(d => d.Followers).Include(d => d.User).ThenInclude(d => d.Followings).Where(u => u.UserId != user.Id).ToList();
            TargetUser = CustomerDetails.FirstOrDefault(c => c.User.Id == Username);

        }
    }
}
