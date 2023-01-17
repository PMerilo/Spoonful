using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.Admin.RecipeManagement
{

    [BindProperties]
    public class updateRecipeModel : PageModel
    {
        private readonly AuthDbContext _db;

        private readonly RecipeService _recipeService;

        public updateRecipeModel(AuthDbContext db, RecipeService recipeService)
        {
            _db = db;
            _recipeService = recipeService;
            
        }

        public IEnumerable<Recipe> Recipes { get; set; }

        public Recipe MyRecipe { get; set; }

        public IActionResult OnGet(int id)
        {
            Recipe recipe = _recipeService.GetRecipeById(id);
            if(recipe != null)
            {
                MyRecipe = recipe;
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
            if (ModelState.IsValid)
            {
                Recipe? recipe = _recipeService.GetRecipeByName(MyRecipe.name);
                if (recipe == null)
                {
                    MyRecipe.Id = id;
                    _recipeService.UpdateRecipe(MyRecipe);
                    TempData["FlashMessage.Type"] = "success";
                    TempData["FlashMessage.Text"] = string.Format("Recipe {0} is updated", MyRecipe.name);
                    return Redirect("/Admin/RecipeManagement");
                }
                else
                {
                    TempData["FlashMessage.Type"] = "danger";
                    TempData["FlashMessage.Text"] = string.Format("Recipe {0} already exist!", MyRecipe.name);
                    return Page();
                }

                
            }
            return Page();
        }


        }
}
