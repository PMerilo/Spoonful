using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;
using Stripe;

namespace Spoonful.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly UserManager<CustomerUser> _userManager;
    private readonly CustomerUserService _customerUserService;

    public IndexModel(ILogger<IndexModel> logger, UserManager<CustomerUser> userManager, CustomerUserService customerUserService)
    {
        _logger = logger;
        _userManager = userManager;
        _customerUserService = customerUserService;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        CustomerUser root = await _userManager.FindByNameAsync("rootuser");
        if (root == null)
        {
            root = new CustomerUser
            {
                UserName = "rootuser",
                Email = "rootuser@spoonful.com",
                EmailConfirmed = true,
                FirstName = "Root",
                LastName = "User"
            };
            var result = await _userManager.CreateAsync(root, "Password@123");
            if (result.Succeeded)
            {
                await _customerUserService.SetUserRoleAsync(root.UserName, Roles.RootUser);
                Console.WriteLine("Rootuser created");
            }

        };
        if (User.Identity.IsAuthenticated)
        {
            CustomerUser user = await _userManager.GetUserAsync(User);
            if (await _userManager.IsInRoleAsync(user, Roles.Admin))
            {
                return Redirect("/Admin");
            }
        }
        return Page();
    }
	public async Task<IActionResult> OnPostDeleteUserAsync(string name)
    {
        var user = await _userManager.FindByNameAsync(name);
        if (user != null)
        {
            await _userManager.DeleteAsync(user);
        }
        return RedirectToPage();
    }
}
