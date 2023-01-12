using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.user.MealKitSubscription
{

    [Authorize]
    [BindProperties]
    public class OrderModel : PageModel
    {
        private readonly AuthDbContext _db;

        private readonly MealKitService _mealKitService;

        public MealKit MyMealKit { get; set; }

        public OrderModel(AuthDbContext db, MealKitService mealKitService)
        {
            _db = db;
            _mealKitService = mealKitService;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                
                _mealKitService.AddMealKit(MyMealKit);
                return Redirect("/user/MealKitSubscription/OrderConfirmed");
            }

            return Page();

        }
    }
}
