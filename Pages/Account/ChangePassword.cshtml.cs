using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using System.ComponentModel.DataAnnotations;

namespace Spoonful.Pages.Account
{
    [BindProperties]
    public class ChangePasswordModel : PageModel
    {
        private readonly SignInManager<CustomerUser> signInManager;
        private readonly UserManager<CustomerUser> userManager;
        public ChangePasswordModel(SignInManager<CustomerUser> signInManager, UserManager<CustomerUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;

        }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords must match")]
        public string ConfirmPassword { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await userManager.GetUserAsync(User);

            var result = await userManager.ChangePasswordAsync(user, Password, NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return Page();
            }

            TempData["FlashMessage.Text"] = "Password changed successfully";
            TempData["FlashMessage.Type"] = "success";

            return Redirect("/Account");
        }
    }
}
