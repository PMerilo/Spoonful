using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Spoonful.Models;
using Spoonful.Services;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;

namespace Spoonful.Pages.Account
{
    public class ChangePhoneModel : PageModel
    {
        private readonly UserManager<CustomerUser> userManager;
        private IWebHostEnvironment _environment;

        private readonly ISmsSender smsSender;
        private readonly INotyfService toastService;
        public ChangePhoneModel(UserManager<CustomerUser> userManager, IWebHostEnvironment environment, ISmsSender smsSender, INotyfService toastService)
        {
            this.userManager = userManager;
            _environment = environment;
            this.smsSender = smsSender;
            this.toastService = toastService;
        }
        [BindProperty]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [BindProperty]
        public string Code { get; set; }
        public bool Sent { get; set; } = false;
        public async Task OnGet()
        {
            var user = await userManager.GetUserAsync(User);
            Phone = user.PhoneNumber;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Phone == null)
            {
                toastService.Error("Phone Number field is required");
                return RedirectToPage();
            }
            var user = await userManager.GetUserAsync(User);

            if (user.PhoneNumber == Phone)
            {
                toastService.Error("Please enter a different phone number.");
                return RedirectToPage();
            }
            var code = await userManager.GenerateChangePhoneNumberTokenAsync(user, Phone);
            var resultSMS = smsSender.SendSmsAsync(
                        Phone,
                        $"This is your verification code: {code}"
                        );
            toastService.Success("Verification SMS sent. Please check your messages.");
            Sent = true;
            return Page();
        }
        public async Task<IActionResult> OnPostVerifyAsync()
        {
            if (!ModelState.IsValid)
            {
                toastService.Error("Phone Number & OTP fields are required");
                return RedirectToPage();
            }
            var user = await userManager.GetUserAsync(User);

            if (user.PhoneNumber == Phone)
            {
                toastService.Error("Please enter a different phone number.");
                return RedirectToPage();
            }

            var result = await userManager.ChangePhoneNumberAsync(user, Phone, Code);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                toastService.Error("Invalid Tokens");
                return RedirectToPage();
            }

            toastService.Success("Successfuly changed phone number");
            return RedirectToPage();
        }
    }
}
