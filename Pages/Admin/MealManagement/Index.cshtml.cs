using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;

namespace Spoonful.Pages.Admin.MealManagement
{
    public class IndexModel : PageModel
    {
        private readonly AuthDbContext _db;

        public IEnumerable<MenuItem> MenuItems { get; set; }

        public IndexModel(AuthDbContext db)
        {
            _db = db;
            
        }

        public void OnGet()
        {
            MenuItems = _db.MenuItem;
        }
    }
}
