using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Spoonful.Models;
using Spoonful.Services;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Spoonful.Pages.Account
{
    public class CreateAdminModel : PageModel
    {
        private readonly AuthDbContext _db;
        private readonly UserManager<CustomerUser> _userManager;
        private readonly SignInManager<CustomerUser> _signInManager;
        private readonly CustomerUserService _customerUserService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly INotyfService _toastService;
        private readonly IEmailService _emailService;

        public CreateAdminModel(AuthDbContext db, UserManager<CustomerUser> userManager, RoleManager<IdentityRole> roleManager, INotyfService notyfService, IEmailService emailService, SignInManager<CustomerUser> signInManager, CustomerUserService customerUserService)
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
                    await _customerUserService.SetUserRoleAsync(user.UserName, Roles.Admin);
                    _toastService.Success("Admin Account Created");
                    await _signInManager.SignInAsync(user, false);
                    return Redirect("/Admin");
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
    public class RegisterAdmin
    {
        //[Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        //[Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

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
    }
}
