using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.user.MealKitSubscription
{
    public class OrderConfirmedModel : PageModel
    {
        private readonly UserManager<CustomerUser> _userManager;
        private readonly AuthDbContext _db;

        private readonly MealKitService _mealKitService;

        public MealKit MyMealKit { get; set; }

        public OrderConfirmedModel(UserManager<CustomerUser> userManager, AuthDbContext db, MealKitService mealKitService)
        {
            _userManager = userManager;
            _db = db;
            _mealKitService = mealKitService;
            
        }

        public void OnGet()
        {
        }
    }
}
