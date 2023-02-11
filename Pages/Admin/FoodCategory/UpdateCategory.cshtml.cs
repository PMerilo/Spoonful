using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Services;
using Spoonful.Models;

namespace Spoonful.Pages.Admin.FoodCategory
{
    public class UpdateCategoryModel : PageModel
    {
        private readonly CategoryService _categoryService;
        private IWebHostEnvironment _environment;

        public UpdateCategoryModel(CategoryService categoryService, IWebHostEnvironment environment)
        {
            _categoryService = categoryService;
            _environment = environment;
        }

        [BindProperty]
        public Category MyCategory { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            Category? category = _categoryService.GetCategoryById(id);
            if (category != null)
            {
                MyCategory = category;
                return Page();
            }
            else
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = string.Format("Category id {0} not found", id);
                return Redirect("/Admin/FoodCategory");
            }
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (ModelState.IsValid)
            {
                Category? category = _categoryService.GetCategoryByName(MyCategory.name);
               
                if (category != null)
                {
                    
                    TempData["FlashMessage.Type"] = "danger";
                    TempData["FlashMessage.Text"] = string.Format("Category {0} already exist!", MyCategory.name);
                    return Page();
                    
                    
                }
                Console.WriteLine(id);
                Console.WriteLine(MyCategory.Id);
                Console.WriteLine(MyCategory.name);
                MyCategory.Id = id;
                _categoryService.UpdateCategory(MyCategory);
                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = string.Format("Category {0} is updated", MyCategory.name);
                return Redirect("/Admin/FoodCategory");
            }
            return Page();
        }
        
    }
}
