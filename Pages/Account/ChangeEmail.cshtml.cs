using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Spoonful.Models;
using Spoonful.Services;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace Spoonful.Pages.Account
{
    public class ChangeEmailModel : PageModel
    {
        private readonly UserManager<CustomerUser> userManager;
        private IWebHostEnvironment _environment;

        private readonly IEmailService emailService;
        private readonly INotyfService toastService;
        public ChangeEmailModel(UserManager<CustomerUser> userManager, IWebHostEnvironment environment, IEmailService emailService, INotyfService toastService)
        {
            this.userManager = userManager;
            _environment = environment;
            this.emailService = emailService;
            this.toastService = toastService;
        }
        [BindProperty]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public async Task OnGet()
        {
            var user = await userManager.GetUserAsync(User);
            Email = user.Email;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                toastService.Error("Email field is required");
                return RedirectToPage();
            }
            var user = await userManager.GetUserAsync(User);

            if (user.Email == Email)
            {
                toastService.Error("Please enter a different email.");
                return RedirectToPage();
            }
            var code = await userManager.GenerateChangeEmailTokenAsync(user, Email);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
            "/Account/Confirmation",
                pageHandler: null,
                values: new { username = user.UserName, code = code, purpose = "ChangeEmail", email = Email },
                protocol: Request.Scheme);
            var resultEmail = emailService.SendEmail(
                        Email,
                        "Spoonful Account Change Of Email Address",
                        $"Please verify your email by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.",
                        null,
                        null);

            if (!resultEmail)
            {
                toastService.Error("Failed to send email");
            }
            toastService.Success("Verification email sent. Please check your email.");
            return RedirectToPage();
        }
    }
}
