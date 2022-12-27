using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Spoonful.Pages.User
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Login LModel { get; set; }

        private readonly SignInManager<CustomerUser> signInManager;
        private readonly UserManager<CustomerUser> userManager;
		public LoginModel(SignInManager<CustomerUser> signInManager, UserManager<CustomerUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;

		}
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                if (LModel.Username.IndexOf('@') > -1)
                {
                    var user = await userManager.FindByEmailAsync(LModel.Username);
                    if (user != null) LModel.Username = user.UserName;
                }

				var identityResult = await signInManager.PasswordSignInAsync(LModel.Username, LModel.Password,
                LModel.RememberMe, false);

				if (identityResult.Succeeded)
                {
                    return RedirectToPage("/Index");
                }
                ModelState.AddModelError("", "Username or Password incorrect");
            }
            return Page();
        }
    }

    public class Login
    {
        [Required]
        [Display(Name = "Email or Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

		[Required]
		public bool RememberMe { get; set; }
    }
}
