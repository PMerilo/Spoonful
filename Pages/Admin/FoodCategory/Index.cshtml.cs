using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.Admin.FoodCategory
{

    [BindProperties]
    public class IndexModel : PageModel   

    {
        private readonly AuthDbContext _db;

        private readonly CategoryService _categoryService;

        public IEnumerable<Category> Categories { get; set; }

        public Category MyCategory { get; set; }

        public IndexModel(AuthDbContext db,CategoryService categoryService)
        {
            _db = db;
            _categoryService = categoryService;
        }

        public void OnGet()
        {
            Categories = _db.Category;
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (ModelState.IsValid)
            {
                Console.WriteLine("My Category ID and Name For Delete");
                Console.WriteLine(MyCategory.Id);
                Console.WriteLine(MyCategory.name);
                _categoryService.DeleteCategory(MyCategory);
                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = string.Format("Category {0} is Deleted", MyCategory.name);
                return Redirect("/Admin/FoodCategory");
            }
            return Page();
        }
    }
}
