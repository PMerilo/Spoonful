using Spoonful.Models;

namespace Spoonful.Services
{
	public class MenuService
	{
	}

	public class CategoryService
	{
		private readonly AuthDbContext _context;

		public CategoryService(AuthDbContext context)
		{
			_context = context;
		}

		public List<Category> GetAll()
		{
			return _context.Category.OrderBy(m=>m.name).ToList();
		}

		public Category? GetCategoryByName(string name)
		{
            Category? category = _context.Category.FirstOrDefault(x => x.name.Equals(name));
            return category;
		}

		public void AddCategory(Category category)
		{
			_context.Category.Add(category);
			_context.SaveChanges();
		}

		public void UpdateCategory(Category category)
		{
			_context.Category.Update(category);
			_context.SaveChanges();
		}
	}
}
