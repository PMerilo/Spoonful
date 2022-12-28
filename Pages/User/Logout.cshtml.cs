using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;

namespace Spoonful.Pages.User
{
    public class LogoutModel : PageModel
    {
		private readonly SignInManager<CustomerUser> signInManager;
		public LogoutModel(SignInManager<CustomerUser> signInManager)
		{
			this.signInManager = signInManager;
		}
		public void OnGet() { }
		public async Task<IActionResult> OnPostLogoutAsync()
		{
			await signInManager.SignOutAsync();
			return RedirectToPage("/user/Login");
		}
		public async Task<IActionResult> OnPostDontLogoutAsync()
		{
			return RedirectToPage("/Index");
		}
    }
}
