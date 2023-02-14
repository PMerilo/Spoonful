using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;

namespace Spoonful.Pages.Admin.UserManagement
{
    public class RolesModel : PageModel
    {
        private readonly AuthDbContext _db;
        private readonly UserManager<CustomerUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesModel(AuthDbContext db, UserManager<CustomerUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public List<IdentityRole> Roles { get; set; }
        public IdentityRole Role { get; set; }
        public List<CustomerUser> UsersinRole { get; set; }
        public List<CustomerUser> UsersNotinRole { get; set; }
        [BindProperty]
        public string SelectedUserName { get; set; }
        [BindProperty]
        public string RoleName { get; set; }

        public async Task<IActionResult> OnGetAsync(string name)
        {
            if (name == "All")
            {
                Roles = _roleManager.Roles.ToList();

            }
            else
            {
                Role = _roleManager.Roles.FirstOrDefault(r => r.Name == name);
                if (Role == null || Role.Name == "Customer" || Role.Name == "Driver")
                {
                    return NotFound();
                }
                UsersinRole = (await _userManager.GetUsersInRoleAsync(Role.Name)).ToList();
                UsersNotinRole = _userManager.Users.Where(u => !UsersinRole.Contains(u) && u.UserDetails.UserType == "Admin").ToList();
            }
            return Page();
        }

        //public IActionResult OnGetRoles()
        //{
        //    Roles = _roleManager.Roles.ToList();
        //    return new JsonResult(Roles);
        //}

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                Roles = _roleManager.Roles.ToList();
                return Page();
            }
            if (!await _roleManager.RoleExistsAsync(RoleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(RoleName));
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDelete(string name)
        {
            var role = await _roleManager.FindByNameAsync(name);
            await _roleManager.DeleteAsync(role);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAddUserToRole(string name)
        {
            var user = await _userManager.FindByNameAsync(SelectedUserName);
            await _userManager.AddToRoleAsync(user, name);
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostDeleteUserFromRole(string name, string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            await _userManager.RemoveFromRoleAsync(user, name);
            return RedirectToPage();
        }
    }
}
