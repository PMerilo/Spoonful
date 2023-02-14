using AspNetCoreHero.ToastNotification.Abstractions;
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
using System.Text.Encodings.Web;

namespace Spoonful.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Login LModel { get; set; }
        public string Email { get; set; }

        private readonly SignInManager<CustomerUser> signInManager;
        private readonly UserManager<CustomerUser> userManager;
        private readonly CustomerUserService _customerUserService;
        private readonly INotyfService toastService;
        private readonly IEmailService emailService;

        public LoginModel(SignInManager<CustomerUser> signInManager, UserManager<CustomerUser> userManager, CustomerUserService customerUserService, INotyfService toastService, IEmailService emailService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            _customerUserService = customerUserService;
            this.toastService = toastService;
            this.emailService = emailService;
        }
        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPostSendEmailAsync()
        {
            var user = await userManager.FindByNameAsync(LModel.Username);
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
            "/Account/Confirmation",
                pageHandler: null,
                values: new { code = code, username = user.UserName },
                protocol: Request.Scheme);

            var resultEmail = emailService.SendEmail(
                user.Email,
                "Spoonful Account Confirmation",
                $"Please verify your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.",
                null,
                null);

            if (!resultEmail)
            {
                toastService.Success("Failed to send email");
            }
            return Page();
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
                        toastService.Error("You cannot log in right now. Please contact the system administator for assistance.");
                        return Page();
                    }
                    if (user.RequirePassChange)
					{
						await signInManager.SignOutAsync();
						var code = await userManager.GeneratePasswordResetTokenAsync(user);
						code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        toastService.Error("Please set your password to continue to login");
                        return RedirectToPage("/Account/ResetPassword", new { code = code, username = user.UserName });
					}
					if (await userManager.IsInRoleAsync(user, Roles.Admin) && await _customerUserService.ValidateLastPassChangedAsync(user.UserName))
					{
						await signInManager.SignOutAsync();
						var code = await userManager.GeneratePasswordResetTokenAsync(user);
						code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
						TempData["FlashMessage.Text"] = "Your password is too old. Please change your password to continue to login";
						TempData["FlashMessage.Type"] = "warning";
						return RedirectToPage("/Account/ResetPassword", new { code = code, username = user.UserName });
					}
					_customerUserService.UpdateLastLogin(user.UserName);
                    toastService.Success("Logged in successfully");
                    if (await userManager.IsInRoleAsync(user, Roles.Admin))
                    {
						return Redirect(ReturnUrl ?? "/Admin");
					}
                    return Redirect(ReturnUrl ?? "/");
                }
                if (identityResult.RequiresTwoFactor)
                {
                    return RedirectToPage("/Account/2FA");
                }
                if (identityResult.IsNotAllowed)
                {
                    TempData["FlashMessage.Text"] = $"You need to verify your email. <a href='/Account/ResendEmailConfirmation'>Click here to send a new verification email</a>";
                    TempData["FlashMessage.Type"] = "warning";
                    ModelState.AddModelError("", "You have not verified your account. Please check your email inbox.");
                    return Page();
                }
                if (identityResult.IsLockedOut)
                {
                    ModelState.AddModelError("", "You have too many failed attempts please try again later");
                    //_auditService.Log(new AuditLog
                    //{
                    //    Action = AuditService.Event.Lockout,
                    //    Description = "This user attempted login on a locked out account /Account/Login",
                    //    Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault(),
                    //    ApplicationUserId = user.Id,
                    //    ApplicationUser = user,
                    //});
                    return Page();
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
