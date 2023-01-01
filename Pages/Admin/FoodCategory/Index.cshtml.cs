using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;

namespace Spoonful.Pages.Admin.FoodCategory
{
    public class IndexModel : PageModel   

    {
        private readonly AuthDbContext _db;

        public IEnumerable<Category> Categories { get; set; }

        public IndexModel(AuthDbContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            Categories = _db.Category;
        }
    }
}
