using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Spoonful.Services;
using AspNetCore.ReCaptcha;
using Spoonful.Models;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace Spoonful.Pages.Account
{
    [BindProperties]
    public class ChangePasswordModel : PageModel
    {
        private readonly SignInManager<CustomerUser> signInManager;
        private readonly UserManager<CustomerUser> userManager;
        private readonly CustomerUserService customerUserService;
        private readonly INotyfService toastService;

        public ChangePasswordModel(SignInManager<CustomerUser> signInManager, UserManager<CustomerUser> userManager, CustomerUserService customerUserService, INotyfService toastService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.customerUserService = customerUserService;
            this.toastService = toastService;
        }

        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                toastService.Error("Passwords do not match");
                return Page();
            }

            if (OldPassword == Password)
            {
                toastService.Error("New password cannot be the same as the current password");
                return Page();
            }
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                toastService.Error("Something Went Wrong");
                return Redirect("/");
            }

            var result = await userManager.ChangePasswordAsync(user, OldPassword, Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    toastService.Success(error.Description);
                    ModelState.AddModelError("", error.Description);
                }
                return Page();
            }


            toastService.Success("Successfully changed password");
            await signInManager.RefreshSignInAsync(user);
			return RedirectToPage("/Account/Login");
            
        }
    }
}
