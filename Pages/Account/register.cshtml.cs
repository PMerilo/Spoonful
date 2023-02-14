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
using Spoonful.Services;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace Spoonful.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private UserManager<CustomerUser> userManager { get; }
        private SignInManager<CustomerUser> signInManager { get; }
        private CustomerUserService _customerUserService { get; }
        private readonly IEmailService emailService;
        private readonly INotyfService toastService;

        [BindProperty]
        public Register RModel { get; set; }
        public RegisterModel(UserManager<CustomerUser> userManager,
        SignInManager<CustomerUser> signInManager, CustomerUserService customerUserService, IEmailService emailService, INotyfService toastService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _customerUserService = customerUserService;
            this.emailService = emailService;
            this.toastService = toastService;
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
                    _customerUserService.UpdateLastLogin(user.UserName);
                    await _customerUserService.SetUserRoleAsync(user.UserName, Roles.Customer);
                    toastService.Success("Created account successfully. Please check your email for account confirmation");
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
                        toastService.Error("Failed to send email");
                    }
                    return RedirectToPage("./login");
                }
                foreach (var error in result.Errors)
                {
                    toastService.Error(error.Description);
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

        [Required]
        public bool Terms { get; set; }

    }

}
