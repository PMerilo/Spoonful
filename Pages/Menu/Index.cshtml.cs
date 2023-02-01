using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.Menu
{
    public class IndexModel : PageModel
    {
        private readonly AuthDbContext _db;

        private readonly MenuItemService _menuItemService;

        public IEnumerable<MenuItem> MenuItems { get; set; }

        public MenuItem MyMenuItem { get; set; }

        public IndexModel(AuthDbContext db, MenuItemService menuItemService)
        {
            _db = db;
            _menuItemService = menuItemService;
           
        }

        public void OnGet()
        {
            MenuItems = _db.MenuItem;
        }
    }
}
