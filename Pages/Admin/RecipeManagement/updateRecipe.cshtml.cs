using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Spoonful.Models;
using Spoonful.Services;
using System.ComponentModel.DataAnnotations;

namespace Spoonful.Pages.Admin.RecipeManagement
{

    [BindProperties]
    public class updateRecipeModel : PageModel
    {
        private readonly AuthDbContext _db;

        private readonly RecipeService _recipeService;
        private IWebHostEnvironment _environment;

        public updateRecipeModel(AuthDbContext db, RecipeService recipeService, IWebHostEnvironment environment)
        {
            _db = db;
            _recipeService = recipeService;
            _environment = environment;
            
        }

        public IEnumerable<Recipe> Recipes { get; set; }

        public string? name { get; set; }

        public string? description { get; set; }

        public double? prepTime { get; set; }

        public string? allergens { get; set; }

        public string? category { get; set; } = "Undefined";
        
        public string? instructions { get; set; }

        public string? ingredients { get; set; }

        public string? ImageURL { get; set; }

        public IFormFile? Upload { get; set; }

        public IActionResult OnGet(int id)
        {

            Recipe recipe = _recipeService.GetRecipeById(id);
            if(recipe != null)
            {
                name = recipe.name;
                description = recipe.description;
                prepTime = recipe.prepTime;
                allergens = recipe.allergens;
                category = recipe.category;
                instructions = recipe.instructions;
                ingredients = recipe.ingredients;
                ImageURL = recipe.ImageURL;

                return Page();
            }

            else
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = string.Format("Recipe Id {0} not found", id);
                return Redirect("/Admin/RecipeManagement");
            }

        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var recipe = _recipeService.GetRecipeById(id);

                if (recipe == null)
                {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = string.Format("Recipe does not exist!", name);
                return Page();
                }
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
                recipe.ImageURL = string.Format("/{0}/{1}", uploadsFolder, imageFile);
            }
            if (ModelState.IsValid)
            {
                recipe.name = name;
                recipe.description = description;
                recipe.allergens = allergens;
                recipe.prepTime = prepTime;
                recipe.category = category;
                recipe.instructions = instructions;
                recipe.ingredients = ingredients;
                _recipeService.UpdateRecipe(recipe);
                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = string.Format("Recipe {0} is updated", recipe.name);
                return Redirect("/Admin/RecipeManagement");
                
            }
            return Page();
        }


        }
}
