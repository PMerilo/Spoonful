using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.Admin.MealKitManagement
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<CustomerUser> _userManager;
        private readonly AuthDbContext _db;
        private readonly MealKitService _mealKitService;

        public IEnumerable<MealKit> MealKits { get; set; }
        public IEnumerable<CustomerUser> CustomerUsers { get; set; }
        public IndexModel(UserManager<CustomerUser> userManager, AuthDbContext db, MealKitService mealKitService)
        {
            _userManager = userManager;
            _db = db;
            _mealKitService = mealKitService;
        }

        public async Task<IActionResult> OnGet()
        {
            CustomerUsers = _db.Users;
            MealKits = _db.MealKit;
            return Page();
        }
    }
}
