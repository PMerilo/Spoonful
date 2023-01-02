using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.Admin.MealManagement
{
    [BindProperties]
    public class AddMealsModel : PageModel
    {
        private readonly AuthDbContext _db;
        
        private readonly MenuItemService _menuItemService;

        public IEnumerable<Category> Categories { get; set; }

        public IEnumerable<MenuItem> MenuItems { get; set; }

       
        public MenuItem MyMenuItem { get; set; }

        public AddMealsModel(AuthDbContext db, MenuItemService menuItemService)
        {
            _db = db;
            _menuItemService = menuItemService;
        }

        public void OnGet()
        {
            Categories = _db.Category;
        
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                MenuItem? menuItem = _menuItemService.GetMenuByName(MyMenuItem.Name);
                if (menuItem != null)
                {
                    TempData["FlashMessage.Type"] = "danger";
                    TempData["FlashMessage.Text"] = string.Format("Menu item {0} already exist!", MyMenuItem.Name);
                    return Page();
                }
                _menuItemService.AddMealItem(MyMenuItem);
                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = string.Format("Menu item {0} is added", MyMenuItem.Name);
                return Redirect("/Admin/MealManagement");
            }
            return Page();
        }
    }
}
