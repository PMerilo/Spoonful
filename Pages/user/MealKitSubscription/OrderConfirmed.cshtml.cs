using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Spoonful.Models;
using Spoonful.Services;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace Spoonful.Pages.user.MealKitSubscription
{
    [Authorize]
    public class OrderConfirmedModel : PageModel
    {
        private readonly UserManager<CustomerUser> _userManager;
        private readonly AuthDbContext _db;
        public readonly IEmailService _emailSender;
        private readonly MealKitService _mealKitService;
        private readonly OrderService _orderService;
        private readonly InvoiceMealKitService _invoiceMealKitService;
        private readonly MealKitSubscriptionLogService _mealKitSubscriptionLogService; 
        private readonly VoucherService _voucherService;



        public OrderConfirmedModel(UserManager<CustomerUser> userManager, AuthDbContext db, MealKitService mealKitService, OrderService orderService, IEmailService emailSender, InvoiceMealKitService invoiceMealKitService, MealKitSubscriptionLogService mealKitSubscriptionLogService, VoucherService voucherService)
        {
            _userManager = userManager;
            _db = db;
            _mealKitService = mealKitService;
            _orderService = orderService;
            _emailSender = emailSender;
            _invoiceMealKitService = invoiceMealKitService;
            _mealKitSubscriptionLogService = mealKitSubscriptionLogService;
            _voucherService = voucherService;
        }

        public async Task<IActionResult> OnGet(string id,string code)
        {
            var user = await _userManager.GetUserAsync(User);
            MealKit? mealkit = _mealKitService.GetMealKitByUserId(user.Id);
            OrderDetails? orderDetails = _orderService.GetOrderDetailsByUserId(user.Id);
            Vouchers? voucher = _voucherService.GetVoucherByCode(code);
            int val = 100;
            if (voucher != null)
            {
                val = (int)(val - voucher.discountAmount);
                voucher.Quantity = voucher.Quantity - 1;
                voucher.Used = voucher.Used + 1;
            }
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
                        double serving = 5.00;

                        
                        double totalCost = (double)(serving * mealkit.noOfPeoplePerWeek * mealkit.noOfServingsPerPerson * mealkit.noOfRecipesPerWeek);
                        totalCost = (totalCost / 100) * val;

                        Invoice invoice = new Invoice() { MenuPreference = mealkit.MenuPreference, noOfRecipesPerWeek = mealkit.noOfRecipesPerWeek , noOfPeoplePerWeek = mealkit.noOfPeoplePerWeek, noOfServingsPerPerson = mealkit.noOfServingsPerPerson, Address = orderDetails.Address,OrderDate = orderDetails.OrderDate, OrderTime = orderDetails.OrderTime, Cost = totalCost, Name = user.FirstName + " " + user.LastName, Email = user.Email, userId = user.Id, mealkitId = mealkit.Id, orderDetailsId = orderDetails.Id, DiscountCodeUsed = voucher.voucherCode};
                        MealKitSubscriptionLog mealKitSubscriptionLog = new MealKitSubscriptionLog() { noOfUsersSubscribed = 1, description = $"{user.UserName} has subscribed to our meal kit plan" };

                        _invoiceMealKitService.AddInvoice(invoice);
                        _mealKitSubscriptionLogService.AddMealKitSubscriptionLog(mealKitSubscriptionLog);
                        _voucherService.UpdateVoucher(voucher);

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
