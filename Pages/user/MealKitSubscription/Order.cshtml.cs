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
        private readonly VoucherService _voucherService;

        public MealKit MyMealKit { get; set; }
        public OrderDetails MyOrderDetails { get;set; }
        public string Vcode { get; set; }
        public OrderModel(AuthDbContext db, MealKitService mealKitService, UserManager<CustomerUser> userManager, OrderService orderService, VoucherService voucherService)
        {
            _db = db;
            _mealKitService = mealKitService;
            _userManager = userManager;
            _orderService = orderService;
            _voucherService = voucherService;
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            //string? username = User.Identity?.Name;
            MealKit? mealkit = _mealKitService.GetMealKitByUserId(user.Id);
            if (mealkit != null)
            {
                if (mealkit.SubscriptionCheck == false)
                {
                    OrderDetails? orderDetails = _orderService.GetOrderDetailsByUserId(user.Id);
                    _mealKitService.DeleteMealKit(mealkit);
                    _orderService.DeleteOrderDetails(orderDetails);
                    return Page();
                }
                return Redirect("/user/CurrentMealKitPlan");
            }
                return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            double serving = 5.00;
            
            int totalServings = (int)(MyMealKit.noOfPeoplePerWeek * MyMealKit.noOfServingsPerPerson * MyMealKit.noOfRecipesPerWeek);
            double totalCost = (double)(serving * MyMealKit.noOfPeoplePerWeek * MyMealKit.noOfServingsPerPerson * MyMealKit.noOfRecipesPerWeek);
            MyMealKit.orderDetailsId = MyOrderDetails.Id;
            Vouchers? voucher = _voucherService.GetVoucherByCode(Vcode);
            Console.WriteLine(Vcode);
            Console.WriteLine("Hello");
            int quantity = 1;
            int deliminator = 100;
            if(voucher != null)
            {
                deliminator = (int)(100 - voucher.discountAmount);
            }
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
                        UnitAmount = (long)totalCost * deliminator,
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Spoonful Meal Kit Subscription",
                            Description = $"{MyMealKit.MenuPreference} Plan," +
                            $" {MyMealKit.noOfServingsPerPerson} Servings for {MyMealKit.noOfPeoplePerWeek}," +
                            $" {totalServings} Total Servings"
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
                    SuccessUrl = domain + $"/user/MealKitSubscription/OrderConfirmed?id={MyOrderDetails.Id}&code={Vcode}",
                    //SuccessUrl = domain + "/OrderConfirmed.cshtml?session_id={CHECKOUT_SESSION_ID}",
                    CancelUrl = domain + "/user/MealKitSubscription/Order",
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

        public async Task<JsonResult> OnPostCheckDiscount(string code)
        {
            var date = DateTime.Now.ToString("dd/MM/yyyy");
            Vouchers? voucher = _voucherService.GetVoucherByCode(code); 
            if (voucher != null)
            {
                var voucherDate = voucher.expiryDate.ToString("dd/MM/yyyy");
                if (voucher.Quantity > 0)
                {
                    if (DateTime.Compare(Convert.ToDateTime(voucherDate), Convert.ToDateTime(date)) > 0)
                    {
                        return new JsonResult(new{ status = "Valid", discountAmt = voucher.discountAmount});
                    }
                    else
                    {
                        return new JsonResult(new { status = "Expired"});
                    }
                }
                else
                {
                    return new JsonResult(new { status = "ran_out"});
                }
            }
            else
            {
                return new JsonResult(new { status = "InvalidCode"});
            }
            System.Diagnostics.Debug.WriteLine(code);
        }
    }
}
