using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;
using Stripe;
using Stripe.Checkout;

namespace Spoonful.Pages.user.MealKitSubscription
{

    [Authorize]
    [BindProperties]
    public class OrderModel : PageModel   
    {
        private readonly UserManager<CustomerUser> _userManager;
        private readonly AuthDbContext _db;

        private readonly MealKitService _mealKitService;
        private readonly OrderService _orderService;
        
        public MealKit MyMealKit { get; set; }

        public OrderDetails MyOrderDetails { get;set; }

        public OrderModel(AuthDbContext db, MealKitService mealKitService, UserManager<CustomerUser> userManager, OrderService orderService)
        {
            _db = db;
            _mealKitService = mealKitService;
            _userManager = userManager;
            _orderService = orderService;
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            MealKit? mealkit = _mealKitService.GetMealKitByUserId(user.Id);
            if (mealkit != null)
            {
                return Redirect("/user/CurrentMealKitPlan");
            }
                return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            MyMealKit.orderDetailsId = MyOrderDetails.Id;
            int quantity = 1;
            if (ModelState.IsValid && MyMealKit.orderDetailsId != null)
            {
                
                
                var domain = "https://localhost:44367";

                //var priceOptions = new PriceListOptions
                //{
                    //LookupKeys = new List<string> {
                    //Request.Form["lookup_key"]
                    //}
                //};
                //var priceService = new PriceService();
                //StripeList<Price> prices = priceService.List(priceOptions);

                var options = new SessionCreateOptions
                {
                    LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = 100,
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Spoonful Meal Kit Subscription",
                            Description = "Spoonful Description"
                        },
                    },
                    Quantity = quantity

                  },
                },
                    PaymentMethodTypes = new List<string>
                {
                  "card"  
                },
                    Mode = "payment",
                    SuccessUrl = domain + "/OrderConfirmed",
                    //SuccessUrl = domain + "/OrderConfirmed.cshtml?session_id={CHECKOUT_SESSION_ID}",
                    CancelUrl = domain + "/Order",
                };
                var service = new SessionService();
                Session session = service.Create(options);

                Response.Headers.Add("Location", session.Url);

                _mealKitService.AddMealKit(MyMealKit);
                _orderService.AddOrderDetails(MyOrderDetails);

                return new StatusCodeResult(303);

                //return Redirect("/user/MealKitSubscription/OrderConfirmed");
            }

            return Page();

        }


    }
}
