using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.Admin.MealKitManagement
{
    public class ViewCustomerBoxModel : PageModel
    {
        private readonly AuthDbContext _db;
        private readonly OrderService _orderService;
        private readonly MealKitService _mealKitService;
        private readonly MealOrderService _mealOrderService;
        private readonly MenuItemService _menuItemService;
        private readonly UserManager<CustomerUser> _userManager;

        public ViewCustomerBoxModel(AuthDbContext db, OrderService orderService, MealKitService mealKitService, MealOrderService mealOrderService, MenuItemService menuItemService, UserManager<CustomerUser> userManager)
        {
            _db = db;
            _orderService = orderService;
            _mealKitService = mealKitService;
            _mealOrderService = mealOrderService;
            _menuItemService = menuItemService;
            _userManager = userManager;
        }

        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<MenuItem> MenuItems { get; set; }
        public MealKit MyMealKit { get; set; }
        public Invoice MyInvoice { get; set; }

        public OrderDetails MyOrderDetails { get; set; }
        public async Task<IActionResult> OnGet()
        {
            return Page();
        }
    }
}
