using Spoonful.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.AspNetCore.DataProtection;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace Spoonful.Pages.Account
{
	[AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
		private readonly SignInManager<CustomerUser> signInManager;
		private IDataProtectionProvider _dataProtectionProvider;
		private readonly UserManager<CustomerUser> userManager; 
		private readonly INotyfService toastService;

        [BindProperty]
        public Register RModel { get; set; }
        public string? ReturnUrl { get; set; }

        public ExternalLoginModel (SignInManager<CustomerUser> signInManager, UserManager<CustomerUser> userManager, IDataProtectionProvider dataProtectionProvider, INotyfService toastService)
		{
			this.signInManager = signInManager;
            this.userManager = userManager;
			_dataProtectionProvider = dataProtectionProvider;
			this.toastService = toastService;
		}
		public IActionResult OnPost(string provider, string? returnUrl = null)
		{
			// Request a redirect to the external login provider.
			var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
			var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
			return new ChallengeResult(provider, properties);
		}

		public async Task<IActionResult> OnGetCallbackAsync(string? returnUrl = null, string? remoteError = null)
		{
			returnUrl = returnUrl ?? Url.Content("~/");
			if (remoteError != null)
			{
				//ErrorMessage = $"Error from external provider: {remoteError}";
				return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
			}
			var info = await signInManager.GetExternalLoginInfoAsync();
			if (info == null)
			{
				//ErrorMessage = "Error loading external login information.";
				return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
			}

			// Sign in the user with this external login provider if the user already has a login.
			var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
			if (result.Succeeded)
			{
                toastService.Success("Successfully logged in!");
                //_logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
			}
			if (result.IsLockedOut)
			{
                toastService.Error("You have been locked out as you had too many invalid attempts. Please try again later");
                return RedirectToPage("./Login");
			}
			else
			{
				// If the user does not have an account, then ask the user to create an account.
				ReturnUrl = returnUrl;
				if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
				{
                    RModel = new Register
					{
						Email = info.Principal.FindFirstValue(ClaimTypes.Email),
						FirstName = info.Principal.FindFirstValue(ClaimTypes.Name),
					};
				}
				return Page();
			}
		}

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            // Get the information about the user from the external login provider
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                //ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            if (ModelState.IsValid)
            {
				var protector = _dataProtectionProvider.CreateProtector("MySecretKey");
				var user = new CustomerUser
                {
                    UserName = RModel.UserName,
                    Email = RModel.Email,
                    EmailConfirmed = true,
					FirstName =  RModel.FirstName,
					LastName = RModel.LastName
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
						await userManager.UpdateSecurityStampAsync(user);
                        toastService.Success("Successfully logged in!");
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    toastService.Error(error.Description);

                }
            }

            //ProviderDisplayName = info.ProviderDisplayName;
            ReturnUrl = returnUrl;
            return Page();
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
            public bool Terms { get; set; }
        }

	}
}
