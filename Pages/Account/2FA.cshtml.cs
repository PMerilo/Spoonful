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
        private readonly IEmailService emailService;
        private UserManager<CustomerUser> userManager { get; }
        private SignInManager<CustomerUser> signInManager { get; }

        public _2FAModel(UserManager<CustomerUser> userManager, SignInManager<CustomerUser> signInManager, ISmsSender smsSender, IEmailService emailService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.smsSender = smsSender;
            this.emailService = emailService;
        }
        [BindProperty]
        public string OTP { get; set; } 
        public List<string> Providers { get; set; }

        [BindProperty]
        public string SelectedProvider { get; set; } 
        public async Task<IActionResult> OnGet()
        {
            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return RedirectToPage("/Index");
            }
            var providers = await userManager.GetValidTwoFactorProvidersAsync(user);
            Providers = providers.ToList();
            return Page();
        }
        public async Task OnPostSendCodeAsync()
        {
            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
            if (SelectedProvider == "SMS")
            {
                var code = await userManager.GenerateTwoFactorTokenAsync(user, SelectedProvider);
                await smsSender.SendSmsAsync(user.PhoneNumber, $"Security Code: {code}");
            } else
            {
                var code = await userManager.GenerateTwoFactorTokenAsync(user, SelectedProvider);
                emailService.SendEmail(
                    user.Email,
                    "Spoonful Two-Factor Authentication Code",
                    $"This is your 2FA Code: {code}",
                    null,
                    null);
            }
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
