using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using Spoonful.Models;
using Microsoft.Win32;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Spoonful.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private UserManager<CustomerUser> userManager { get; }
        private SignInManager<CustomerUser> signInManager { get; }

        [BindProperty]
        public Register RModel { get; set; }
        public RegisterModel(UserManager<CustomerUser> userManager,
        SignInManager<CustomerUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public void OnGet()
        {
        }

        //Save data into the database
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = new CustomerUser()
                {
                    UserName = RModel.UserName,
                    FirstName = RModel.FirstName,
                    LastName = RModel.LastName,
                    Email = RModel.Email
                };
                var result = await userManager.CreateAsync(user, RModel.Password);
                if (result.Succeeded)
                {
                    //var userclaims = await userManager.GetClaimsAsync(user);
                    //await userManager.RemoveClaimAsync(user, );
                    //await userManager.AddClaimAsync(user, new Claim(ClaimTypes.NameIdentifier, RModel.UserName));
                    await signInManager.SignInAsync(user, false);
                    TempData["FlashMessage.Text"] = "Created account successfully";
                    TempData["FlashMessage.Type"] = "success";
                    return RedirectToPage("/Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return Page();
        }

    }

    public class Register
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        //[Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        //[Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords must match")]
        public string ConfirmPassword { get; set; }
    }

}
