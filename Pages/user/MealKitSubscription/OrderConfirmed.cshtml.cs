using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Spoonful.Models;
using Spoonful.Services;
using System.Text.Encodings.Web;
using System.Text;

namespace Spoonful.Pages.user.MealKitSubscription
{
    public class OrderConfirmedModel : PageModel
    {
        private readonly UserManager<CustomerUser> _userManager;
        private readonly AuthDbContext _db;
        public readonly IEmailService _emailSender;
        private readonly MealKitService _mealKitService;
        private readonly OrderService _orderService;

       

        public OrderConfirmedModel(UserManager<CustomerUser> userManager, AuthDbContext db, MealKitService mealKitService, OrderService orderService, IEmailService emailSender)
        {
            _userManager = userManager;
            _db = db;
            _mealKitService = mealKitService;
            _orderService = orderService;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> OnGet(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            MealKit? mealkit = _mealKitService.GetMealKitByUserId(user.Id);
            OrderDetails? orderDetails = _orderService.GetOrderDetailsByUserId(user.Id);
            if (mealkit != null && orderDetails != null)
            {
                if(orderDetails.Id == id)
                {
                    if(mealkit.SubscriptionCheck == false && orderDetails.SubscriptionCheck == false)
                    {
                        mealkit.SubscriptionCheck = true;
                        orderDetails.SubscriptionCheck = true;
                        _mealKitService.UpdateMealKit(mealkit);
                        _orderService.UpdateOrderDetails(orderDetails);

                        _emailSender.SendEmail(
                            user.Email,
                            "Spoonful Meal Kit Subscription",
                            $"Hello, {user.UserName} You have subscribed to our meal kit plan successfully. If you have any issues with any substitution, or either an ingredient or a Meal Kit, please contact us via ticket through customer support on our website.",
                            null,
                            null);

                        await _db.SaveChangesAsync();
                    }
                    return Page();
                }
                else
                {
                    return Redirect("/user/CurrentMealKitPlan");
                }

                

            }

            return Redirect("/user/CurrentMealKitPlan");


        }
    }
}
