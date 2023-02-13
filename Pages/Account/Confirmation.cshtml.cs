using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Spoonful.Models;
using Spoonful.Services;
using System.Text;

namespace Spoonful.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmationModel : PageModel
    {
        private readonly SignInManager<CustomerUser> signInManager;
        private readonly UserManager<CustomerUser> userManager;
        private readonly IEmailService emailService;
        private readonly INotyfService toastService;
        public ConfirmationModel(SignInManager<CustomerUser> signInManager, UserManager<CustomerUser> userManager, IEmailService emailService, INotyfService toastService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.emailService = emailService;
            this.toastService = toastService;

        }
        public async Task OnGetAsync(string username, string code)
        {
            var user = await userManager.FindByNameAsync(username);

            if (user == null)
            {
                TempData["FlashMessage.Text"] = "Invalid Tokens";
                TempData["FlashMessage.Type"] = "danger";
            }

            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                TempData["FlashMessage.Text"] = "Invalid Tokens";
                TempData["FlashMessage.Type"] = "danger";
            }

            TempData["FlashMessage.Text"] = "Successfully reset password!";
            TempData["FlashMessage.Type"] = "success";
        }
    }
}
