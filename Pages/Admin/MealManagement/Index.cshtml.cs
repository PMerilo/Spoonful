using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.Admin.MealManagement
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        private readonly AuthDbContext _db;

        private readonly MenuItemService _menuItemService;

        public IEnumerable<MenuItem> MenuItems { get; set; }

        public MenuItem MyMenuItem { get; set;}

        public IndexModel(AuthDbContext db, MenuItemService menuItemService)
        {
            _db = db;
            _menuItemService = menuItemService;
            
        }

        public void OnGet()
        {
            MenuItems = _db.MenuItem;
        }

        public async Task<IActionResult> OnPostAsync()
        {

            
            Console.WriteLine("My Menu ID and Name For Delete");
            Console.WriteLine(MyMenuItem.Id);
            Console.WriteLine(MyMenuItem.Name);
            _menuItemService.DeleteMenuItem(MyMenuItem);
            TempData["FlashMessage.Type"] = "success";
            TempData["FlashMessage.Text"] = string.Format("Menu item {0} is Deleted", MyMenuItem.Name);
            return Redirect("/Admin/MealManagement");
            
            
            
        }
    }
}
