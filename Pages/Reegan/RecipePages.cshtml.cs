using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.Reegan
{
    public class RecipePagesModel : PageModel
    {
        private readonly AuthDbContext _db;

        private readonly RecipeService _recipeService;

        public IEnumerable<Recipe> Recipes { get; set; }

        public Recipe MyRecipe { get; set; }

        public RecipePagesModel(AuthDbContext db, RecipeService recipeService)
        {
            _db = db;
            _recipeService = recipeService;

        }

        public void OnGet()
        {
            Recipes = _db.Recipe;
        }
    }
}
