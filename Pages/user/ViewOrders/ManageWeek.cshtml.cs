using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;
using System.Globalization;

namespace Spoonful.Pages.user.ViewOrders
{
    [Authorize]
    [BindProperties]
    public class ManageWeekModel : PageModel
    {
        private readonly AuthDbContext _db;

        private readonly OrderService _orderService;
        private readonly MealKitService _mealKitService;
        private readonly MealOrderService _mealOrderService;
        private readonly MenuItemService _menuItemService;
        private readonly UserManager<CustomerUser> _userManager;
        private readonly CategoryService _categoryService;
        private readonly InvoiceMealKitService _invoiceMealKitService;

        public ManageWeekModel(AuthDbContext db, OrderService orderService, MealKitService mealKitService, MealOrderService mealOrderService, MenuItemService menuItemService, UserManager<CustomerUser> userManager, CategoryService categoryService, InvoiceMealKitService invoiceMealKitService)
        {
            _db = db;
            _orderService = orderService;
            _mealKitService = mealKitService;
            _mealOrderService = mealOrderService;
            _menuItemService = menuItemService;
            _userManager = userManager;
            _categoryService = categoryService;
            _invoiceMealKitService = invoiceMealKitService;
        }

        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<MenuItem> MenuItems { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public MealKit MyMealKit { get; set; }

        public Invoice MyInvoice { get; set; }

        [BindProperty]
        public Order MyOrder { get; set; }

        [BindProperty]
        public MenuItem MyMenuItem { get; set; }
        [BindProperty]
        public OrderDetails MyOrderDetails { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Categories = _db.Category;
            var user = await _userManager.GetUserAsync(User);
            MealKit? mealkit = _mealKitService.GetMealKitByUserId(user.Id);
            OrderDetails? orderDetails = _orderService.GetOrderDetailsByUserId(user.Id);


            

            if (mealkit == null || orderDetails == null)
            {
                return Redirect("/user/CurrentMealKitPlan");
            }

            string currentDate = DateTime.Now.ToString("dddd, dd MMMM yyyy");
            
            if (DateTime.Parse(orderDetails.DeliveryDate, CultureInfo.InvariantCulture) <= DateTime.Parse(currentDate, CultureInfo.InvariantCulture))
            {
                orderDetails.DeliveryDate = DateTime.Now.AddDays(7).ToString("dddd, dd MMMM yyyy");
                _orderService.UpdateOrderDetails(orderDetails);
                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = ($"Your time remaining to edit your box has expired, your previous order has been placed successfully");
                return Redirect("/user/ViewOrders/ManageWeek");
            }

            

            if (mealkit.Status == false)
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = ($"Your Meal Plan is Currently Paused Please Unpause your plan to start managing your orders.");
                return Redirect("/user/CurrentMealKitPlan");
            }

            MyMealKit = mealkit;
            MyOrderDetails = orderDetails;
            Invoice? invoice = _invoiceMealKitService.GetInvoiceByMealKitId(mealkit.Id);

            if(invoice == null)
            {
                return Redirect("/user/CurrentMealKitPlan");
            }

            MyInvoice = invoice;

            MenuItems = _db.MenuItem;
            MenuItems = MenuItems.Where(X => X.Archived == false);
            MenuItems = MenuItems.Where(X => X.MenuPreference == mealkit.MenuPreference);

            Orders = _db.Order;
            Orders = Orders.Where(X => X.OwnerID == user.Id);
            var checkMealsReset = false;
            foreach(var i in Orders)
            {
                if(i.MenuPreference != mealkit.MenuPreference)
                {
                    checkMealsReset = true;
                    _mealOrderService.DeleteOrder(i);
                }
                
            }
            if (checkMealsReset)
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = ($"Some of your order items has been removed due to changing your meal kit preferences.");
                return Redirect("/user/ViewOrders/ManageWeek");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAddmealAsync() 
        {
            MenuItem menuItem = _menuItemService.GetMenuById(MyMenuItem.Id);
            var user = await _userManager.GetUserAsync(User);
            Orders = _db.Order;
            Orders = Orders.Where(X => X.OwnerID == user.Id);
            MealKit? mealkit = _mealKitService.GetMealKitByUserId(user.Id);
            MyMealKit = mealkit;
            if (Orders.Count() >= MyMealKit.noOfRecipesPerWeek)
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = string.Format($"You have reached the minimum amount of meals for the week");
                return Redirect("/user/ViewOrders/ManageWeek");
            }

            if (menuItem != null)
            {
                
                
                Order order = new Order() { Name = menuItem.Name, Description = menuItem.Description, Tags = menuItem.Tags, MenuPreference = menuItem.MenuPreference, Category = menuItem.Category, ImageURL = menuItem.ImageURL, OwnerID = user.Id };
                _mealOrderService.AddOrder(order);
                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = string.Format($"{menuItem.Name} Added to your box");
            }
            else
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = string.Format("Menu Item not found");
            }


            return Redirect("/user/ViewOrders/ManageWeek");
        }

        public async Task<IActionResult> OnPostRemovemealAsync()
        {
            
            
            Order order = _mealOrderService.GetOrderId(MyOrder.Id);
            var user = await _userManager.GetUserAsync(User);
            if (order != null)
            {
               
                _mealOrderService.DeleteOrder(order);
                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = string.Format($"{order.Name} Removed from your box");
            }
            else
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = string.Format("Menu Item not found");
            }


            return Redirect("/user/ViewOrders/ManageWeek");
        }
    }
}
