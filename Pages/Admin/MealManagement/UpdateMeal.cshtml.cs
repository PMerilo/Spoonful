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
        public IEnumerable<Recipe> Recipes { get; set; }
        private IWebHostEnvironment _environment;

        public MenuItem MyMenuItem { get; set; }

        [BindProperty]
        public IFormFile? Upload { get; set; }

        public UpdateMealModel(AuthDbContext db, MenuItemService menuItemService, IWebHostEnvironment environment)
        {

            _db = db;
            _menuItemService = menuItemService;
            _environment = environment;
        }

        public IActionResult OnGet(int id)
        {
            Recipes = _db.Recipe;
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
            var uploadsFolder = "uploads";
            if (MyMenuItem.ImageURL != null && Upload !=null)
            {
                var oldImageFile = Path.GetFileName(MyMenuItem.ImageURL);
                var oldImagePath = Path.Combine(
                _environment.ContentRootPath, "wwwroot", uploadsFolder, oldImageFile);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
                Console.WriteLine(Upload);
                Console.WriteLine("Upload Here");
                if (Upload != null)
                {
                    if (Upload.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("Upload", "File size cannot exceed 2MB.");
                        return Page();
                    }

                    var imageFile = Guid.NewGuid() + Path.GetExtension(Upload.FileName);
                    var imagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, imageFile);
                    using var fileStream = new FileStream(imagePath, FileMode.Create);
                    await Upload.CopyToAsync(fileStream);
                    MyMenuItem.ImageURL = string.Format("/{0}/{1}", uploadsFolder, imageFile);
                }
                

                
            }
            if (ModelState.IsValid)
            {
                MenuItem? menuItem = _menuItemService.GetMenuByName(MyMenuItem.Name);
                
                if (menuItem != null)
                {
                    TempData["FlashMessage.Type"] = "danger";
                    TempData["FlashMessage.Text"] = string.Format("Menu item {0} already exist!", MyMenuItem.Name);
                    return Redirect($"/Admin/MealManagement/UpdateMeal?id={id}");
                }

                else
                {
                    if (MyMenuItem.RecipeId != null)
                    {
                        MyMenuItem.Archived = false;
                    }
                    MyMenuItem.Id = id;
                    _menuItemService.UpdateMenuItem(MyMenuItem);
                    TempData["FlashMessage.Type"] = "success";
                    TempData["FlashMessage.Text"] = string.Format("Menu item {0} is updated", MyMenuItem.Name);
                    return Redirect("/Admin/MealManagement");
                }
                
            }
            return Redirect($"/Admin/MealManagement/UpdateMeal?id={id}");
        }
    }
}
