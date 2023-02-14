using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Spoonful.Models;
using Spoonful.Services;
namespace Spoonful.Pages.Drivers
{
    public class IndexModel : PageModel
    {
        private readonly AuthDbContext _db;
        private readonly UserManager<CustomerUser> _userManager;
        public IndexModel(AuthDbContext db, UserManager<CustomerUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public DriverDetails driverDetails { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            driverDetails = _db.DriverDetails.Include(u => u.User).FirstOrDefault(u => u.User.UserName == user.UserName);
            return Page();
        }
    }
}
