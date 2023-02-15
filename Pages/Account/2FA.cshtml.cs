using Spoonful.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Services;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace Spoonful.Pages.Account
{
    public class _2FAModel : PageModel
    {

        private readonly ISmsSender smsSender;
        private readonly IEmailService emailService;
        private readonly INotyfService toastService;
        private UserManager<CustomerUser> userManager { get; }
        private SignInManager<CustomerUser> signInManager { get; }

        public _2FAModel(UserManager<CustomerUser> userManager, SignInManager<CustomerUser> signInManager, ISmsSender smsSender, IEmailService emailService, INotyfService toastService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.smsSender = smsSender;
            this.emailService = emailService;
            this.toastService = toastService;
        }
        [BindProperty]
        public string OTP { get; set; } 
        [BindProperty]
        public bool RememberMe { get; set; } 
        public List<string> Providers { get; set; }
        public bool SentCode { get; set; }

        [BindProperty]
        public string SelectedProvider { get; set; } 
        public async Task<IActionResult> OnGet(bool rememberMe)
        {
            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return RedirectToPage("/Index");
            }
            var providers = await userManager.GetValidTwoFactorProvidersAsync(user);
            Providers = providers.ToList();
            RememberMe = rememberMe;
            return Page();
        }
        public async Task<IActionResult> OnPostVerifyAsync()
        {
            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
            var providers = await userManager.GetValidTwoFactorProvidersAsync(user);
            Providers = providers.ToList();
            if (OTP == null)
            {
                toastService.Error("Please enter your OTP");
                SentCode = true;
                return Page();
            }
            switch (SelectedProvider)
            {
                case "Authenticator":
                    var authenticatorCode = OTP.Replace(" ", string.Empty).Replace("-", string.Empty);
                    var result = await signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, RememberMe, false);
                    if (result.Succeeded)
                    {
                        toastService.Success("Successfully logged in!");
                        await userManager.UpdateSecurityStampAsync(user);
                        return LocalRedirect("/");
                    }
                    else if (result.IsLockedOut)
                    {
                        toastService.Error("You have been locked out as you had too many invalid attempts. Please try again later");
                        return RedirectToPage("./login");
                    }
                    break;
                default:
                    var resultSignIn = await signInManager.TwoFactorSignInAsync(SelectedProvider, OTP, RememberMe, false);
                    if (resultSignIn.Succeeded)
                    {
                        toastService.Success("Successfully logged in!");
                        await userManager.UpdateSecurityStampAsync(user);
                        return RedirectToPage("/Index");
                    }
                    else if (resultSignIn.IsLockedOut)
                    {
                        toastService.Error("You have been locked out as you had too many invalid attempts. Please try again later");
                        return RedirectToPage("./login");
                    }
                    break;
            }
            ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
            toastService.Error("Invalid OTP");
            SentCode = true;

            return Page();
            //        var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
            //        var providers = await userManager.GetValidTwoFactorProvidersAsync(user);
            //        var result = await signInManager.TwoFactorSignInAsync(providers.FirstOrDefault(), OTP, false, false);
            //        if (result.Succeeded)
            //        {
            //            TempData["FlashMessage.Text"] = "Successfully Logged In";
            //            TempData["FlashMessage.Type"] = "success";
            //await userManager.UpdateSecurityStampAsync(user);
            //return RedirectToPage("/Home");
            //        }
            //        TempData["FlashMessage.Text"] = "Invalid OTP";
            //        TempData["FlashMessage.Type"] = "danger";
            //        return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }
            switch (SelectedProvider)
            {
                case "SMS":
                    var smscode = await userManager.GenerateTwoFactorTokenAsync(user, SelectedProvider);
                    await smsSender.SendSmsAsync(user.PhoneNumber, $"Security Code: {smscode}");
                    break;
                case "Authenticator":
                    
                    break;
                default:
                    var emailcode = await userManager.GenerateTwoFactorTokenAsync(user, SelectedProvider);
                    emailService.SendEmail(
                        user.Email,
                        "Spoonful Two-Factor Authentication Code",
                        $"This is your 2FA Code: {emailcode}",
                        null,
                        null);
                    break;
            }
            SentCode = true;
            return Page();
        }
        
    }
}
