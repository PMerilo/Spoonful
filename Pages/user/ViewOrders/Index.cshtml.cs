using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.user.ViewOrders
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly AuthDbContext _db;
        private readonly OrderService _orderService;
        private readonly MealKitService _mealKitService;
        private readonly MealOrderService _mealOrderService;
        private readonly MenuItemService _menuItemService;
        private readonly UserManager<CustomerUser> _userManager;
        private readonly InvoiceMealKitService _invoiceMealKitService;

        public IndexModel(AuthDbContext db, OrderService orderService, MealKitService mealKitService, MealOrderService mealOrderService, MenuItemService menuItemService, UserManager<CustomerUser> userManager, InvoiceMealKitService invoiceMealKitService)
        {
            _db = db;
            _orderService = orderService;
            _mealKitService = mealKitService;
            _mealOrderService = mealOrderService;
            _menuItemService = menuItemService;
            _userManager = userManager;
            _invoiceMealKitService = invoiceMealKitService;
        }
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<MenuItem> MenuItems { get; set; }
        public MealKit MyMealKit { get; set; }
        public Invoice MyInvoice { get; set; }

        public OrderDetails MyOrderDetails { get; set; }


        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            MealKit? mealkit = _mealKitService.GetMealKitByUserId(user.Id);
            OrderDetails? orderDetails = _orderService.GetOrderDetailsByUserId(user.Id);
            
            Orders = _db.Order;
            Orders = Orders.Where(X => X.OwnerID == user.Id);
            if (mealkit == null || orderDetails == null)
            {
                return Redirect("/user/CurrentMealKitPlan");
            }
            if(mealkit == null)
            {
                return Redirect("/user/CurrentMealKitPlan");
                
            }
            Invoice? invoice = _invoiceMealKitService.GetInvoiceByMealKitId(mealkit.Id);


            if (mealkit.Status == false)
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = ($"Your Meal Plan is Currently Paused Please Unpause your plan to start managing your orders.");
                return Redirect("/user/CurrentMealKitPlan");
            }


            MyMealKit = mealkit;
            MyInvoice = invoice;
            MyOrderDetails = orderDetails;

            if(Orders.Count() != mealkit.noOfRecipesPerWeek)
            {
                /*foreach (MenuItem item in _db.MenuItem)
                {

                    if (item.MenuPreference == mealkit.MenuPreference)
                    {
                        Order? order = new Order() { MenuPreference = item.MenuPreference, Category = item.Category, Description = item.Description, ImageURL = item.ImageURL, Tags = item.Tags, OwnerID = user.Id };
                        _mealOrderService.AddOrder(order);
                    }
                    //figure out how to add meals automatically without bugs check later
                    if(Orders.Count() == mealkit.noOfRecipesPerWeek)
                    {
                        break;
                    }
                    
                }
                */
                if (Orders.Count() != mealkit.noOfRecipesPerWeek)
                {
                    if (Orders.Count() > mealkit.noOfRecipesPerWeek)
                    {
                        TempData["FlashMessage.Type"] = "danger";
                        TempData["FlashMessage.Text"] = ($"Please Remove {Orders.Count() - mealkit.noOfRecipesPerWeek} Recipes for the week to save changes");
                        return Redirect("/user/ViewOrders/ManageWeek");
                    }

                    TempData["FlashMessage.Type"] = "danger";
                    TempData["FlashMessage.Text"] = ($"Please Select Your {mealkit.noOfRecipesPerWeek} Recipes for the week to save changes");
                    return Redirect("/user/ViewOrders/ManageWeek");
                }

                
                    
            }

            return Page();
        }
    }
}
