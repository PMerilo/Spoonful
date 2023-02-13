using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Spoonful.Models;
using Spoonful.Services;
using System.Globalization;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Spoonful.Pages.Admin.UserManagement
{
    public class IndexModel : PageModel
    {
        private readonly AuthDbContext _db;
        private readonly UserManager<CustomerUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly INotyfService _toastService;
        private readonly IEmailService _emailService;

        public IndexModel(AuthDbContext db, UserManager<CustomerUser> userManager, RoleManager<IdentityRole> roleManager, INotyfService notyfService, IEmailService emailService)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _toastService = notyfService;
            _emailService = emailService;
        }

        public List<CustomerUser> Users { get; set; }
        public List<CustomerDetails> CustomerDetails { get; set; }
        public List<AdminDetails> AdminDetails { get; set; }
        public List<DriverDetails> DriverDetails { get; set; }
        public List<IdentityRole> RoleList { get; set; }

        [BindProperty]
        public List<string> RoleInput { get; set; }
        [Required]
        [BindProperty]
        public string UserEmail { get; set; }

        public async Task OnGet()
        {
            Users = _userManager.Users.Include(u => u.UserDetails).ToList();
            CustomerDetails = _db.CustomerDetails.Include(d => d.User).ToList();
            AdminDetails = _db.AdminDetails.Include(d => d.User).ToList();
            DriverDetails = _db.DriverDetails.Include(d => d.User).ToList();
            RoleList = _roleManager.Roles.Where(r => r.Name != "RootUser" && r.Name != "Admin" && r.Name != "Customer" && r.Name != "Driver").ToList();
        }

        public async Task<IActionResult> OnPostDeleteUserAsync(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            await _userManager.DeleteAsync(user);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAddRolesAsync(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            var currentRoles = await _userManager.GetRolesAsync(user);
            currentRoles = currentRoles.Where(r => r != "RootUser" && r != "Admin" && r != "Customer" && r != "Driver").ToList();
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            var result = await _userManager.AddToRolesAsync(user, RoleInput);
            if (result.Succeeded)
            {
                TempData["FlashMessage.Text"] = "Added roles successfully";
                TempData["FlashMessage.Type"] = "success";
            }
            foreach (var error in result.Errors)
            {
                Console.WriteLine(error);
            }
            return RedirectToPage();

        }

        public async Task<IActionResult> OnPostAdminCreateUser()
        {
            if (!ModelState.IsValid)
            {
                Users = _userManager.Users.Include(u => u.UserDetails).ToList();
                CustomerDetails = _db.CustomerDetails.Include(d => d.User).ToList();
                AdminDetails = _db.AdminDetails.Include(d => d.User).ToList();
                DriverDetails = _db.DriverDetails.Include(d => d.User).ToList();
                RoleList = _roleManager.Roles.Where(r => r.Name != "RootUser" && r.Name != "Admin" && r.Name != "Customer" && r.Name != "Driver").ToList();
                _toastService.Error("Email field is required");
                return Page();
            }
            var newUser = new CustomerUser
            {
                Email = UserEmail,
                UserName = UserEmail
            };
            var result = await _userManager.CreateAsync(newUser);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    _toastService.Error($"Something went wrong - {error.Description}");
                }
                return RedirectToPage();
            }
            var resultRole = await _userManager.AddToRolesAsync(newUser, RoleInput);
            if (!resultRole.Succeeded)
            {
                foreach (var error in resultRole.Errors)
                {
                    _toastService.Error($"Something went wrong - {error.Description}");
                }
                return RedirectToPage();
            }
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
            "/Account/CreateAdmin",
            pageHandler: null,
                values: new { code = code, username = newUser.UserName },
                protocol: Request.Scheme);

            var emailresult = _emailService.SendEmail(
                UserEmail,
                "Spoonful Admin Setup",
                $"Please set up your admin account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.",
                null,
                null);

            if (!emailresult)
            {
                _toastService.Error("Email failed to send");
            }
            _toastService.Success("Sent instructions to email");
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDriverCreateUser()
        {
            if (!ModelState.IsValid)
            {
                Users = _userManager.Users.Include(u => u.UserDetails).ToList();
                CustomerDetails = _db.CustomerDetails.Include(d => d.User).ToList();
                AdminDetails = _db.AdminDetails.Include(d => d.User).ToList();
                DriverDetails = _db.DriverDetails.Include(d => d.User).ToList();
                RoleList = _roleManager.Roles.Where(r => r.Name != "RootUser" && r.Name != "Admin" && r.Name != "Customer" && r.Name != "Driver").ToList();
                _toastService.Error("Email field is required");
                return Page();
            }
            var newUser = new CustomerUser
            {
                Email = UserEmail,
                UserName = UserEmail
            };
            var result = await _userManager.CreateAsync(newUser);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    _toastService.Error($"Something went wrong - {error.Description}");
                }
                return RedirectToPage();
            }
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
            "/Account/CreateDriver",
            pageHandler: null,
                values: new { code = code, username = newUser.UserName },
                protocol: Request.Scheme);

            var emailresult = _emailService.SendEmail(
                UserEmail,
                "Spoonful Driver Setup",
                $"Please set up your driver account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.",
                null,
                null);

            if (!emailresult)
            {
                _toastService.Error("Email failed to send");
            }
            _toastService.Success("Sent instructions to email");
            return RedirectToPage();
        }
    }
}
