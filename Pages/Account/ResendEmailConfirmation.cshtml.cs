using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using AspNetCoreHero.ToastNotification.Abstractions;
using Spoonful.Models;
using Spoonful.Services;
using Microsoft.AspNetCore.Authorization;

namespace Spoonful.Pages.Account
{
    [AllowAnonymous]
    public class ResendEmailConfirmationModel : PageModel
    {
        private UserManager<CustomerUser> userManager { get; }
        private SignInManager<CustomerUser> signInManager { get; }
        private CustomerUserService _customerUserService { get; }
        private readonly IEmailService emailService;
        private readonly INotyfService toastService;

        public ResendEmailConfirmationModel(UserManager<CustomerUser> userManager,
        SignInManager<CustomerUser> signInManager, CustomerUserService customerUserService, IEmailService emailService, INotyfService toastService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _customerUserService = customerUserService;
            this.emailService = emailService;
            this.toastService = toastService;
        }
        [BindProperty]
        public string Email { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                toastService.Success("Verification email sent. Please check your email.");
                return Page();
            }
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
            "/Account/Confirmation",
                pageHandler: null,
                values: new { username = user.UserName, code = code },
                protocol: Request.Scheme);
            var resultEmail = emailService.SendEmail(
                        user.Email,
                        "Spoonful Account Confirmation",
                        $"Please verify your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.",
                        null,
                        null);

            if (!resultEmail)
            {
                toastService.Error("Failed to send email");
            }
            toastService.Success("Verification email sent. Please check your email.");
            return Page();
        }
    }
}
