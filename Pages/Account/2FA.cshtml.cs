using Spoonful.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Services;

namespace Spoonful.Pages.Account
{
    public class _2FAModel : PageModel
    {

        private readonly ISmsSender smsSender;
        private UserManager<CustomerUser> userManager { get; }
        private SignInManager<CustomerUser> signInManager { get; }

        public _2FAModel(UserManager<CustomerUser> userManager, SignInManager<CustomerUser> signInManager, ISmsSender smsSender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.smsSender = smsSender;
        }
        [BindProperty]
        public string OTP { get; set; } 
        public async Task<IActionResult> OnGet()
        {
            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return RedirectToPage("/Index");
            }
            var providers = await userManager.GetValidTwoFactorProvidersAsync(user);
            var code = await userManager.GenerateTwoFactorTokenAsync(user, providers.FirstOrDefault());
            await smsSender.SendSmsAsync(user.PhoneNumber, $"Security Code: {code}");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
            var providers = await userManager.GetValidTwoFactorProvidersAsync(user);
            var result = await signInManager.TwoFactorSignInAsync(providers.FirstOrDefault(), OTP, false, false);
            if (result.Succeeded)
            {
                TempData["FlashMessage.Text"] = "Successfully Logged In";
                TempData["FlashMessage.Type"] = "success";
				await userManager.UpdateSecurityStampAsync(user);
				return RedirectToPage("/Home");
            }
            TempData["FlashMessage.Text"] = "Invalid OTP";
            TempData["FlashMessage.Type"] = "danger";
            return Page();
        }
    }
}
