using Spoonful.Models;

namespace Spoonful.Services
{
	public class RecipeService
	{
        private readonly AuthDbContext _context;
        public RecipeService(AuthDbContext context)
        {
            _context = context;
        }

        public List<Recipe> GetAll()
        {
            return _context.Recipe.OrderBy(x => x.name).ToList();
        }

        public Recipe? GetRecipeByName(string name)
        {
            Recipe? recipe = _context.Recipe.FirstOrDefault(x => x.name.Equals(name));
            return recipe;
        }

        public Recipe? GetRecipeById(int id)
        {
            Recipe? recipe = _context.Recipe.FirstOrDefault(x => x.Id.Equals(id));
            return recipe;
        }

        public void AddRecipe(Recipe recipe)
        {
            _context.Recipe.Add(recipe);
            _context.SaveChanges();
        }

        public void UpdateRecipe(Recipe recipe)
        {
            _context.Update(recipe);
            _context.SaveChanges();
        }

        public void DeleteRecipe(Recipe recipe)
        {
            _context.Remove(recipe);
            _context.SaveChanges();
        }
    }
}
