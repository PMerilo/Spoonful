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
        private readonly OrderService _orderService;

       

        public OrderConfirmedModel(UserManager<CustomerUser> userManager, AuthDbContext db, MealKitService mealKitService, OrderService orderService)
        {
            _userManager = userManager;
            _db = db;
            _mealKitService = mealKitService;
            _orderService = orderService;
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            MealKit? mealkit = _mealKitService.GetMealKitByUserId(user.Id);
            OrderDetails? orderDetails = _orderService.GetOrderDetailsByUserId(user.Id);
            if (mealkit != null)
            {
                return Page();

            }

            return Redirect("/user/CurrentMealKitPlan");


        }
    }
}
