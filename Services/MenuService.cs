using Spoonful.Models;

namespace Spoonful.Services
{
	public class MenuItemService
	{
		private readonly AuthDbContext _context;
		public MenuItemService(AuthDbContext context)
		{
			_context = context;
		}

		public List<MenuItem> GetAll()
		{
			return _context.MenuItem.OrderBy(x => x.Name).ToList();
		}

		public MenuItem? GetMenuByName(string name)
		{
			MenuItem? menuItem = _context.MenuItem.FirstOrDefault(x => x.Name.Equals(name));
			return menuItem;
		}

        public MenuItem? GetMenuById(int id)
        {
            MenuItem? menuItem = _context.MenuItem.FirstOrDefault(x => x.Id.Equals(id));
            return menuItem;
        }

		public void AddMealItem(MenuItem menuItem)
		{
			_context.MenuItem.Add(menuItem);
			_context.SaveChanges();
		}

		public void UpdateMenuItem(MenuItem menuItem)
		{
			_context.Update(menuItem);
			_context.SaveChanges();
		}
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

        public Category? GetCategoryById(int id)
        {
            Category? category = _context.Category.FirstOrDefault(x => x.Id.Equals(id));
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
