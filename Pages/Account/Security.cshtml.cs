using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Stripe;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;

namespace Spoonful.Pages.Account
{
    public class SecurityModel : PageModel
    {
        private readonly UserManager<CustomerUser> userManager;
        private IWebHostEnvironment _environment;
        private readonly UrlEncoder _urlEncoder;
        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        public SecurityModel(UserManager<CustomerUser> userManager, IWebHostEnvironment environment, UrlEncoder urlEncoder)
        {
            this.userManager = userManager;
            _environment = environment;
            _urlEncoder = urlEncoder;
        }

        public CustomerUser CurrentUser { get; set; }

        [BindProperty]
        [Required]
        [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Verification Code")]
        public string Code { get; set; }

        [TempData]
        public string[] RecoveryCodes { get; set; }

        public string AuthenticatorUri { get; set; }

        public string SharedKey { get; set; }
        public List<string> Providers { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user != null)
            {

                CurrentUser = user;
                await LoadSharedKeyAndQrCodeUriAsync(user);
                var providers = await userManager.GetValidTwoFactorProvidersAsync(user);
                Providers = providers.ToList();
                return Page();
            }
            else
            {
                return RedirectToPage("/Error");
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadSharedKeyAndQrCodeUriAsync(user);
                return Page();
            }

            // Strip spaces and hyphens
            var verificationCode = Code.Replace(" ", string.Empty).Replace("-", string.Empty);

            var is2faTokenValid = await userManager.VerifyTwoFactorTokenAsync(
                user, userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

            if (!is2faTokenValid)
            {
                ModelState.AddModelError("Input.Code", "Verification code is invalid.");
                await LoadSharedKeyAndQrCodeUriAsync(user);
                return Page();
            }

            await userManager.SetTwoFactorEnabledAsync(user, true);
            var userId = await userManager.GetUserIdAsync(user);

            if (await userManager.CountRecoveryCodesAsync(user) == 0)
            {
                var recoveryCodes = await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
                RecoveryCodes = recoveryCodes.ToArray();
                return RedirectToPage();
            }
            else
            {
                return RedirectToPage();
            }
        }
        private async Task LoadSharedKeyAndQrCodeUriAsync(CustomerUser user)
        {
            // Load the authenticator key & QR code URI to display on the form
            var unformattedKey = await userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await userManager.GetAuthenticatorKeyAsync(user);
            }

            SharedKey = FormatKey(unformattedKey);

            var email = await userManager.GetEmailAsync(user);
            AuthenticatorUri = GenerateQrCodeUri(email, unformattedKey);
        }

        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.AsSpan(currentPosition, 4)).Append(' ');
                currentPosition += 4;
            }
            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.AsSpan(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                AuthenticatorUriFormat,
                _urlEncoder.Encode("Spoonful"),
                _urlEncoder.Encode(email),
                unformattedKey);
        }

        public async Task<IActionResult> OnPostDisableAsync()
        {
            var user = await userManager.GetUserAsync(User);
            await userManager.SetTwoFactorEnabledAsync(user, false);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEnableAsync()
        {
            var user = await userManager.GetUserAsync(User);
            await userManager.SetTwoFactorEnabledAsync(user, true);
            return RedirectToPage();
        }
    }
}
