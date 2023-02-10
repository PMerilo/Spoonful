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
        private IWebHostEnvironment _environment;

        public IEnumerable<Category> Categories { get; set; }

        public IEnumerable<MenuItem> MenuItems { get; set; }

        [BindProperty]
        public IFormFile? Upload { get; set; }


        public MenuItem MyMenuItem { get; set; }

        public AddMealsModel(AuthDbContext db, MenuItemService menuItemService, IWebHostEnvironment environment)
        {
            _db = db;
            _menuItemService = menuItemService;
            _environment = environment;
        }

        public void OnGet()
        {
            Categories = _db.Category;

            //if (Categories == null && TempData["Categories"] !=null )
            //{
            //    Console.WriteLine("Categories Info Here");
            //    Categories = TempData["Categories"] as IEnumerable<Category>;
            //    Console.WriteLine(Categories);
            //}
        
        }

        public async Task<IActionResult> OnPostAsync()
        {
            

            if (ModelState.IsValid)
            {
                if (Upload != null)
                {
                    if (Upload.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("Upload", "File size cannot exceed 2MB.");
                        return Page();
                    }
                    var uploadsFolder = "uploads";
                    var imageFile = Guid.NewGuid() + Path.GetExtension(Upload.FileName);
                    var imagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, imageFile);
                    using var fileStream = new FileStream(imagePath, FileMode.Create);
                    await Upload.CopyToAsync(fileStream);
                    MyMenuItem.ImageURL = string.Format("/{0}/{1}", uploadsFolder, imageFile);
                }
                MenuItem? menuItem = _menuItemService.GetMenuByName(MyMenuItem.Name);
                if (menuItem != null)
                {
                    TempData["FlashMessage.Type"] = "danger";
                    TempData["FlashMessage.Text"] = string.Format("Meal Item {0} already exist!", MyMenuItem.Name);
                    return Page();
                }
                _menuItemService.AddMealItem(MyMenuItem);
                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = string.Format("Meal Item {0} is added", MyMenuItem.Name);
                return Redirect("/Admin/MealManagement");
            }
            //TempData["Categories"] = _db.Category;
            TempData["error"] = "Missing Form Inputs";
            TempData["FlashMessage.Type"] = "danger";
            TempData["FlashMessage.Text"] = ("Missing Form Inputs");
            return Redirect("/Admin/MealManagement/AddMeals");
        }
    }
}
