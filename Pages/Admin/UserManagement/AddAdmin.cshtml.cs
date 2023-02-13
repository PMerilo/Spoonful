 using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;
using System.ComponentModel.DataAnnotations;

namespace Spoonful.Pages.Admin.UserManagement
{
    public class AddAdminModel : PageModel
    {
		private readonly UserManager<CustomerUser> _userManager;
		private readonly CustomerUserService _userService;
		public AddAdminModel(UserManager<CustomerUser> userManager, CustomerUserService userService)
		{
			_userManager = userManager;
			_userService = userService;
		}

		[BindProperty]
        public Register RModel { get; set; }
        public void OnGet()
        {
        }

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var user = new CustomerUser()
			{
				UserName = RModel.UserName,
				RequirePassChange = RModel.RequirePassChange
			};
			var result = await _userManager.CreateAsync(user, RModel.Password);
			if (result.Succeeded)
			{
				//var userclaims = await userManager.GetClaimsAsync(user);
				//await userManager.RemoveClaimAsync(user, );
				//await userManager.AddClaimAsync(user, new Claim(ClaimTypes.NameIdentifier, RModel.UserName));
				await _userService.SetUserRoleAsync(user.UserName, Roles.Admin);
				TempData["FlashMessage.Text"] = "Created account successfully";
				TempData["FlashMessage.Type"] = "success";
				return RedirectToPage("Index");
			}
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}
			return Page();
		}

		public class Register
		{
			[Required]
			[Display(Name = "Username")]
			public string UserName { get; set; }

			[Required]
			[DataType(DataType.Password)]
			public string Password { get; set; }

			[Required]
			[DataType(DataType.Password)]
			[Compare(nameof(Password), ErrorMessage = "Passwords must match")]
			public string ConfirmPassword { get; set; }
			public bool RequirePassChange { get; set; }
		}
	}
}
