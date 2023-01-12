using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Build.Framework;
using System.Text.Encodings.Web;
using System.Text;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.Account
{
    public class ForgetPasswordModel : PageModel
    {
        public readonly EmailService _emailSender;
        public readonly UserManager<CustomerUser> _userManager;

        public ForgetPasswordModel(EmailService emailSender, UserManager<CustomerUser> userManager)
        {
            _emailSender = emailSender;
            _userManager = userManager;

        }

        [BindProperty]
        [Required]
        public string? Email { get; set; }
        public void OnGet()
        {
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = await _userManager.FindByEmailAsync(Email);
            TempData["FlashMessage.Text"] = $"A reset password email will be sent to {user.Email} if it is valid" ;
            
            if (user == null)
            {
                TempData["FlashMessage.Type"] = "danger";
                return Page();
            }
            //var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            //var callbackUrl = Url.Page(
            //"/Account/ResetPassword",
            //    pageHandler: null,
            //    values: new { code },
            //    protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(
                Email,
                "Reset Password",
                $"Please reset your password by <a href=''>clicking here</a>.");
            TempData["FlashMessage.Type"] = "info";
            return Page();
        }
    }
}
