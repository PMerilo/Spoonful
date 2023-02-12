using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Spoonful.Models;
using Spoonful.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;

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
                    var userByEmail = await userManager.FindByEmailAsync(LModel.Username);
                    if (userByEmail != null) LModel.Username = userByEmail.UserName;
                }

				var user = await userManager.FindByNameAsync(LModel.Username);
				

				var identityResult = await signInManager.PasswordSignInAsync(LModel.Username, LModel.Password,
                LModel.RememberMe, false);

                if (identityResult.Succeeded)
                {
                    if (user.isDisabled)
                    {
                        await signInManager.SignOutAsync();
                        TempData["FlashMessage.Text"] = $"You cannot log in right now. Please contact the system administator for assistance.";
                        TempData["FlashMessage.Type"] = "danger";
                        return Page();
                    }
                    if (user.RequirePassChange)
					{
						await signInManager.SignOutAsync();
						var code = await userManager.GeneratePasswordResetTokenAsync(user);
						code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
						TempData["FlashMessage.Text"] = "Please set your password to continue to login";
						TempData["FlashMessage.Type"] = "warning";
						return RedirectToPage("/Account/ResetPassword", new { code = code, username = user.UserName });
					}
					if (await _customerUserService.ValidateLastPassChangedAsync(user.UserName))
					{
						await signInManager.SignOutAsync();
						var code = await userManager.GeneratePasswordResetTokenAsync(user);
						code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
						TempData["FlashMessage.Text"] = "Your password is too old. Please change your password to continue to login";
						TempData["FlashMessage.Type"] = "warning";
						return RedirectToPage("/Account/ResetPassword", new { code = code, username = user.UserName });
					}
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
