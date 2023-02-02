using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.user.ViewOrders
{
    public class IndexModel : PageModel
    {
        private readonly AuthDbContext _db;

        private readonly OrderService _orderService;
        private readonly MealKitService _mealKitService;
        private readonly MealOrderService _mealOrderService;

        public IndexModel(AuthDbContext db, OrderService orderService, MealKitService mealKitService, MealOrderService mealOrderService)
        {
            _db = db;
            _orderService = orderService;
            _mealKitService = mealKitService;
            _mealOrderService = mealOrderService;
        }



        
        public void OnGet()
        {
        }
    }
}
