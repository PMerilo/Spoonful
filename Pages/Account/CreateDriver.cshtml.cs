using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Spoonful.Models;
using Spoonful.Services;
using System.Text;

namespace Spoonful.Pages.Account
{
    public class CreateDriverModel : PageModel
    {
        private readonly AuthDbContext _db;
        private readonly UserManager<CustomerUser> _userManager;
        private readonly SignInManager<CustomerUser> _signInManager;
        private readonly CustomerUserService _customerUserService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly INotyfService _toastService;
        private readonly IEmailService _emailService;
        public CreateDriverModel(AuthDbContext db, UserManager<CustomerUser> userManager, RoleManager<IdentityRole> roleManager, INotyfService notyfService, IEmailService emailService, SignInManager<CustomerUser> signInManager, CustomerUserService customerUserService)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _toastService = notyfService;
            _emailService = emailService;
            _signInManager = signInManager;
            _customerUserService = customerUserService;
        }
        [BindProperty]
        public RegisterAdmin RAdmin { get; set; }

        public async Task<IActionResult> OnGetAsync(string code, string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return NotFound();
            }
            RAdmin = new RegisterAdmin
            {
                UserName = user.Email
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string username)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(username);
                user.FirstName = RAdmin.FirstName;
                user.LastName = RAdmin.LastName;
                user.UserName = RAdmin.UserName;
                var resultPass = await _userManager.AddPasswordAsync(user, RAdmin.Password);
                var result = await _userManager.UpdateAsync(user);
                if (resultPass.Succeeded && result.Succeeded)
                {
                    _customerUserService.UpdateLastLogin(user.UserName);
                    await _customerUserService.SetUserRoleAsync(user.UserName, Roles.Driver);
                    var driver = _db.DriverDetails.Include(u => u.User).FirstOrDefault(u => u.User.UserName == username);
                    driver.HourlyRate = 15;
                    _db.SaveChanges();
                    _toastService.Success("Driver Account Created");
                    await _signInManager.SignInAsync(user, false);
                    return Redirect("/Driver");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                foreach (var error in resultPass.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return Page();
        }
    }
}
