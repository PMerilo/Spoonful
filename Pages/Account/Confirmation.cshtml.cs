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
        public IdentityResult Result { get; set; }
        public async Task OnGetAsync(string username, string code, string purpose = ConfirmationPurpose.ConfirmEmail, string? email = null)
        {
            var user = await userManager.FindByNameAsync(username);

            if (user == null)
            {
                TempData["FlashMessage.Text"] = "Invalid Tokens";
                TempData["FlashMessage.Type"] = "danger";
            }

            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            switch (purpose)
            {
                case ConfirmationPurpose.ConfirmEmail:
                    Result = await userManager.ConfirmEmailAsync(user, token);
                    break;

                case ConfirmationPurpose.ChangeEmail:
                    Result = await userManager.ChangeEmailAsync(user, email, token);
                    break;
                default:
                    break;
            }
            if (!Result.Succeeded)
            {
                foreach (var error in Result.Errors)
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
    public class ConfirmationPurpose
    {
        public const string ConfirmEmail = "ConfirmEmail";
        public const string ConfirmPhone = "ConfirmPhone";

        public const string ChangePhone = "ChangePhone";
        public const string ChangeEmail = "ChangeEmail";
    }
}
