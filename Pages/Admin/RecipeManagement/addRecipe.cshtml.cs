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

        public IEnumerable<Recipe> Recipes { get; set; }

        public Recipe MyRecipe { get; set; } 
        public addRecipeModel(AuthDbContext db, RecipeService recipeService)
        {
            _db = db;
            _recipeService = recipeService;

        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
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
