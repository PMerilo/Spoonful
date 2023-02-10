using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.Menu
{
    public class MenuItemModel : PageModel
    {
        public readonly AuthDbContext _db;

        public readonly MenuItemService _menuItemService;

        public MenuItem MyMenuItem { get; set; }

        public MenuItemModel(AuthDbContext db, MenuItemService menuItemService)
        {
            _db = db;
            _menuItemService = menuItemService;

        }

        public IActionResult OnGet(int id)
        {
            MenuItem? menuItem = _menuItemService.GetMenuById(id);
            if (menuItem != null)
            {
                MyMenuItem = menuItem;

                return Page();
            }
            else
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = string.Format("Menu Item Id {0} not found", id);
                return Redirect("/Admin/ViewMeals");
            }
            return Page();
        }
    }
}
