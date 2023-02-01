using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;

namespace Spoonful.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<CustomerUser> signInManager;
        public LogoutModel(SignInManager<CustomerUser> signInManager)
        {
            this.signInManager = signInManager;
        }
        public void OnGet() { }

        public async Task<IActionResult> OnGetLogoutAsync()
        {
            await signInManager.SignOutAsync();
            TempData["FlashMessage.Text"] = "Logged out successfully";
            TempData["FlashMessage.Type"] = "success";
            return RedirectToPage("Login");
        }
        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await signInManager.SignOutAsync();
            return RedirectToPage("Login");
        }
        public async Task<IActionResult> OnPostDontLogoutAsync()
        {
            return RedirectToPage("/Index");
        }
    }
}
