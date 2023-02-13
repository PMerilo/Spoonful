using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Spoonful.Models;
using Spoonful.Services;
using System.Security.Claims;

namespace Spoonful.Pages.Admin.UserManagement
{
    public class IndexModel : PageModel
    {
        private readonly AuthDbContext _db;
        private readonly UserManager<CustomerUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IndexModel(AuthDbContext db, UserManager<CustomerUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public List<CustomerUser> Users { get; set; }
        public List<CustomerDetails> CustomerDetails { get; set; }
        public List<AdminDetails> AdminDetails { get; set; }
        public List<DriverDetails> DriverDetails { get; set; }
        public List<IdentityRole> RoleList { get; set; }

        public async Task OnGet()
        {
            Users = _userManager.Users.Include(u => u.UserDetails).ToList();
            CustomerDetails = _db.CustomerDetails.Include(d => d.User).ToList();
            AdminDetails = _db.AdminDetails.Include(d => d.User).ToList();
            DriverDetails = _db.DriverDetails.Include(d => d.User).ToList();
            //Ddeets = _db.Users.Include(d => d.UserDetails).Where(d => d.UserDetails.UserType == Roles.Driver);
            RoleList = _roleManager.Roles.Where(r => r.Name != "RootUser" && r.Name != "Admin" && r.Name != "Customer" && r.Name != "Driver").ToList();
        }

        public async Task<IActionResult> OnPostDeleteUserAsync(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            await _userManager.DeleteAsync(user);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAddRolesAsync(string name, IFormCollection form)
        {
            var user = await _userManager.FindByNameAsync(name);
            var currentRoles = await _userManager.GetRolesAsync(user);
            currentRoles = currentRoles.Where(r => r != "RootUser" && r != "Admin" && r != "Customer" && r != "Driver").ToList();
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            var keys = form.Keys.ToList();
            var roles = new List<string>();
            foreach (var key in keys)
            {
                if (key == "__RequestVerificationToken")
                {
                    continue;
                }

                roles.Add(key);
                
            }
            var result = await _userManager.AddToRolesAsync(user, roles);
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
    }
}
