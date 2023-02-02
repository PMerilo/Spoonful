using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.Admin.RecipeManagement
{

    [BindProperties]
    public class addRecipeModel : PageModel
    {
        private readonly AuthDbContext _db;

        private readonly RecipeService _recipeService;
        private IWebHostEnvironment _environment;

        public IEnumerable<Recipe> Recipes { get; set; }

        public Recipe MyRecipe { get; set; } 

        public IFormFile? Upload { get; set; }
        public addRecipeModel(AuthDbContext db, RecipeService recipeService, IWebHostEnvironment environment)
        {
            _db = db;
            _recipeService = recipeService;
            _environment = environment;

        }


        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
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
                MyRecipe.ImageURL = string.Format("/{0}/{1}", uploadsFolder, imageFile);
            }
            Recipe? recipe = _recipeService.GetRecipeByName(MyRecipe.name);
            if(recipe != null)
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = string.Format("Recipe item {0} already exist!", MyRecipe.name);
                return Page();
            }
            else
            {
                _recipeService.AddRecipe(MyRecipe);
                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = string.Format("Menu item {0} is added", MyRecipe.name);
                return Redirect("/Admin/RecipeManagement");
            }


            return Page();
        }

    }
}
