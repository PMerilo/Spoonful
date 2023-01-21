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

        public void DeleteMenuItem(MenuItem menuItem)
        {
            _context.Remove(menuItem);
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

		public void DeleteCategory(Category category)
		{
			_context.Category.Remove(category);
			_context.SaveChanges();
		}
	}

	public class MealKitService
	{
        private readonly AuthDbContext _context;

        public MealKitService(AuthDbContext context)
        {
            _context = context;
        }

        public List<MealKit> GetAll()
        {
            return _context.MealKit.OrderBy(m => m.Id).ToList();
        }


        public MealKit? GetMealKitById(int id)
        {
            MealKit? mealkit = _context.MealKit.FirstOrDefault(x => x.Id.Equals(id));
            return mealkit;
        }

        public MealKit? GetMealKitByUserId(string id)
        {
            MealKit? mealkit = _context.MealKit.FirstOrDefault(x => x.userId.Equals(id));
            return mealkit;
        }

        public void AddMealKit(MealKit mealkit)
        {
            _context.MealKit.Add(mealkit);
            _context.SaveChanges();
        }

        public void UpdateMealKit(MealKit mealkit)
        {
            _context.MealKit.Update(mealkit);
            _context.SaveChanges();
        }

        public void DeleteMealKit(MealKit mealkit)
        {
            _context.MealKit.Remove(mealkit);
            _context.SaveChanges();
        }
    }
}
