using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using Spoonful.Models;
using Spoonful.Services;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreHero.ToastNotification.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Spoonful.Pages.Account
{
    [AllowAnonymous]
    public class ForgetPasswordModel : PageModel
    {
        public readonly IEmailService _emailSender;
        public readonly UserManager<CustomerUser> _userManager;
        private readonly INotyfService toastService;


        public ForgetPasswordModel(IEmailService emailSender, UserManager<CustomerUser> userManager, INotyfService toastService)
        {
            _emailSender = emailSender;
            _userManager = userManager;
            this.toastService = toastService;

        }

        [BindProperty]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public void OnGet()
        {
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                toastService.Error($"Email Field is required");
                return Page();
            }
            var user = await _userManager.FindByEmailAsync(Email);
            toastService.Information($"A reset password email will be sent to {Email} if it is valid");

            if (user == null)
            {
                return Page();
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
            "/Account/ResetPassword",
                pageHandler: null,
                values: new { code = code, username = user.UserName },
                protocol: Request.Scheme);

            var result = _emailSender.SendEmail(
                Email,
                "Reset Password",
                $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.",
                null,
                null);
            
            if (!result)
            {
                toastService.Error($"Failed to send email.");

            }
            return Page();
        }
    }
}
