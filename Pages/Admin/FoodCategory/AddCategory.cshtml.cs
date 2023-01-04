using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Spoonful.Services;
using static Spoonful.Services.CategoryService;
using Spoonful.Models;

namespace Spoonful.Pages.Admin.FoodCategory
{
    public class AddCategoryModel : PageModel
    {
        
        private readonly CategoryService _categoryService;
        private IWebHostEnvironment _environment;

        public AddCategoryModel(CategoryService categoryService, IWebHostEnvironment environment)
        {
            _categoryService = categoryService;
            _environment = environment;
        }

        [BindProperty]
        public Category MyCategory { get; set; }
        
        public async Task<IActionResult> OnPostAsync()
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
                _categoryService.AddCategory(MyCategory);
                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = string.Format("Category {0} is added", MyCategory.name);
                return Redirect("/Admin/FoodCategory");
            }
            return Page();
        }

        
        public void OnGet()
        {
           // MyCategory = _categoryService.GetAll();
        }
    }
}
