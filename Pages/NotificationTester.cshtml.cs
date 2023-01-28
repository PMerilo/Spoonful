using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;
using System.Linq;

namespace Spoonful.Pages
{
    public class NotificationTesterModel : PageModel
    {
        private readonly NotificationService _notificationService;
        private readonly UserManager<CustomerUser> _userManager;

        public NotificationTesterModel (NotificationService notificationService, UserManager<CustomerUser> userManager)
        {
            _notificationService = notificationService;
            _userManager = userManager;
        }

        public static List<CustomerUser> Users { get; set; } = new List<CustomerUser> ();

        [BindProperty]
        public string UserName { get; set; }

        [BindProperty]
        public string Title { get; set; }

        [BindProperty]
        public string Body { get; set; }

        [BindProperty]
        public string Url { get; set; }
        public async Task OnGet()
        {
            Users = _userManager.Users.ToList();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                TempData["FlashMessage.Text"] = "Invalid Form";
                TempData["FlashMessage.Type"] = "danger";
                return Page();
            }
            var notification = new Notification()
            {
                Title = Title,
                Body = Body,
                Url = Url,
            };
            await _notificationService.SendNotificationAsync(notification, UserName);
            TempData["FlashMessage.Text"] = "Success";
            TempData["FlashMessage.Type"] = "success";
            return Page();
        }
    }
}
