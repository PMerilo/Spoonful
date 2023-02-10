using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Spoonful.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Login LModel { get; set; }

        private readonly SignInManager<CustomerUser> signInManager;
        private readonly UserManager<CustomerUser> userManager;
        private readonly CustomerUserService _customerUserService;

        public LoginModel(SignInManager<CustomerUser> signInManager, UserManager<CustomerUser> userManager, CustomerUserService customerUserService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            _customerUserService = customerUserService;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string? ReturnUrl)
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
					var user = await userManager.FindByNameAsync(LModel.Username);
                    _customerUserService.UpdateLastLogin(user.UserName);
                    //Create the security context
                    //var claims = new List<Claim>
                    //{
                    //    //new Claim(ClaimTypes.Name, LModel.Username),
                    //    new Claim(ClaimTypes.NameIdentifier, LModel.Username),
                    //    //new Claim(ClaimTypes.Email, "c@c.com")
                    //};
                    //var i = new ClaimsIdentity(claims, "MyCookieAuth");
                    //ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(i);
                    //await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);
                    TempData["FlashMessage.Text"] = $"Logged in successfully";
                    TempData["FlashMessage.Type"] = "success";
					if (await userManager.IsInRoleAsync(user, Roles.Admin))
                    {
						return Redirect(ReturnUrl ?? "/Admin");
					}
                    return Redirect(ReturnUrl ?? "/");
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
