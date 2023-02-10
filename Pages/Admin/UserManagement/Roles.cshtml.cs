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
        [BindProperty]
        public string RoleName { get; set; }

        public void OnGet()
        {
            Roles = _roleManager.Roles.ToList();
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
    }
}
