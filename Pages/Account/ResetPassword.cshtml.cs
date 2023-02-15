using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Spoonful.Models;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Spoonful.Pages.Account
{
    [AllowAnonymous]
    [BindProperties]
    public class ResetPasswordModel : PageModel
    {
        private readonly SignInManager<CustomerUser> signInManager;
        private readonly UserManager<CustomerUser> userManager;
        private readonly INotyfService toastService;

        public ResetPasswordModel(SignInManager<CustomerUser> signInManager, UserManager<CustomerUser> userManager, INotyfService toastService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.toastService = toastService;

        }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords must match")]
        public string ConfirmPassword { get; set; }
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync(string code, string username)
        {
            if (!ModelState.IsValid)
            {
                toastService.Error("Passwords do not match");
                return Page();
            }
            var user = await userManager.FindByNameAsync(username);

            if (user == null)
            {
                toastService.Error("Invalid Tokens");
                return RedirectToPage("/Account/Login");
            }

            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await userManager.ResetPasswordAsync(user, token, Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                toastService.Error("Invalid Tokens");
                return Page();
            }
            toastService.Success("Successfully reset password!");
            return RedirectToPage("/Account/Login");
            
        }
    }
}
