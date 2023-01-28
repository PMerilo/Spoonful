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
        public ResetPasswordModel(SignInManager<CustomerUser> signInManager, UserManager<CustomerUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;

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
                TempData["FlashMessage.Text"] = "Passwords do not match";
                TempData["FlashMessage.Type"] = "danger";
                return Page();
            }
            var user = await userManager.FindByNameAsync(username);

            if (user == null)
            {
                TempData["FlashMessage.Text"] = "Invalid Tokens";
                TempData["FlashMessage.Type"] = "danger";
                return Redirect("/");
            }

            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await userManager.ResetPasswordAsync(user, token, Password);

            if (!result.Succeeded)
            {
                TempData["FlashMessage.Text"] = "Invalid Tokens";
                TempData["FlashMessage.Type"] = "danger";
                return Page();
            }
            TempData["FlashMessage.Text"] = "Successfully reset password!";
            TempData["FlashMessage.Type"] = "success";
            return RedirectToPage("/Account/Login");
            
        }
    }
}
