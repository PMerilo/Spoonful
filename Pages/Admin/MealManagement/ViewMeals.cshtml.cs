using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.Admin.MealManagement
{
    
    public class ViewMealsModel : PageModel
    {
        private readonly AuthDbContext _db;

        private readonly MenuItemService _menuItemService;

        public IEnumerable<MenuItem> MenuItems { get; set; }

        public MenuItem MyMenuItem { get; set; }

        public ViewMealsModel(AuthDbContext db, MenuItemService menuItemService)
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
