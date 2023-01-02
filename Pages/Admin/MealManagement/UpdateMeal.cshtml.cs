using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.Admin.MealManagement
{
    [BindProperties]
    public class UpdateMealModel : PageModel
    {
        private readonly AuthDbContext _db;

        private readonly MenuItemService _menuItemService;

        public IEnumerable<Category> Categories { get; set; }

        public IEnumerable<MenuItem> MenuItems { get; set; }


        public MenuItem MyMenuItem { get; set; }

        public UpdateMealModel(AuthDbContext db, MenuItemService menuItemService)
        {
            _db = db;
            _menuItemService = menuItemService;
            
        }

        public IActionResult OnGet(int id)
        {
            Categories = _db.Category;
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
                return Redirect("/Admin/MealManagement");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (ModelState.IsValid)
            {
                MyMenuItem.Id = id;
                _menuItemService.UpdateMenuItem(MyMenuItem);
                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = string.Format("Menu item {0} is updated", MyMenuItem.Name);
                return Redirect("/Admin/MealManagement");
            }
            return Page();
        }
    }
}
