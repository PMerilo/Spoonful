using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;

namespace Spoonful.Pages.user
{
    [Authorize]
    [BindProperties]
    public class CurrentMealKitPlanModel : PageModel
    {
        private readonly AuthDbContext _db;

        private readonly MealKitService _mealKitService;

        private readonly UserManager<CustomerUser> _userManager;

        private readonly OrderService _orderService;

        private readonly InvoiceMealKitService _invoiceMealKitService;

        public readonly IEmailService _emailSender;

        private IWebHostEnvironment _environment;


        public CurrentMealKitPlanModel(AuthDbContext db, MealKitService mealKitService, UserManager<CustomerUser> userManager, OrderService orderService, InvoiceMealKitService invoiceMealKitService, IEmailService emailSender, IWebHostEnvironment environment)
        {
            _db = db;
            _mealKitService = mealKitService;
            _userManager = userManager;
            _orderService = orderService;
            _invoiceMealKitService = invoiceMealKitService;
            _emailSender = emailSender;
            _environment = environment;
        }

        [BindProperty]
        public MealKit MyMealKit { get; set; }


        public OrderDetails MyOrderDetails { get; set; }

        public Invoice MyInvoice { get; set; }


        public async Task<IActionResult> OnGet()
        {

            var user = await _userManager.GetUserAsync(User);
            MealKit? mealkit = _mealKitService.GetMealKitByUserId(user.Id);
            OrderDetails? orderDetails = _orderService.GetOrderDetailsByUserId(user.Id);
            
            if (mealkit != null)
            {
                Invoice? invoice = _invoiceMealKitService.GetInvoiceByMealKitId(mealkit.Id);
                MyInvoice = invoice;

                Console.WriteLine("Meal Kit Information");
                Console.WriteLine(mealkit.Id);
                Console.WriteLine(mealkit.userId);
                Console.WriteLine(mealkit.MenuPreference);
                MyMealKit = mealkit;
                MyOrderDetails = orderDetails;
            }


            return Page();


        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            MealKit? mealkit = _mealKitService.GetMealKitByUserId(user.Id);
            
            if (mealkit != null)
            {
                Invoice? invoice = _invoiceMealKitService.GetInvoiceByMealKitId(mealkit.Id);
                

                mealkit.noOfPeoplePerWeek = MyMealKit.noOfPeoplePerWeek;
                mealkit.MenuPreference = MyMealKit.MenuPreference;
                mealkit.noOfServingsPerPerson = MyMealKit.noOfServingsPerPerson;
                mealkit.noOfRecipesPerWeek = MyMealKit.noOfRecipesPerWeek;
                mealkit.userId = user.Id;


                double serving = 5.00;

                
                double totalCost = (double)(serving * mealkit.noOfPeoplePerWeek * mealkit.noOfServingsPerPerson * mealkit.noOfRecipesPerWeek);
                invoice.Cost = totalCost;

                _invoiceMealKitService.UpdateInvoice(invoice);

                Console.WriteLine(MyMealKit);
                Console.WriteLine(MyMealKit.Id);
                Console.WriteLine(MyMealKit.MenuPreference);
                MyMealKit.Id = mealkit.Id;
                MyMealKit.userId = user.Id;
                Console.WriteLine("Updated Meal Kit");

                _mealKitService.UpdateMealKit(mealkit);
                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = ("Your Meal Kit Plan Has Been Updated Successfully");
                return Redirect("/user/CurrentMealKitPlan");
            }




            return Page();
        }

        public async Task<IActionResult> OnPostPauseAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            MealKit? mealkit = _mealKitService.GetMealKitByUserId(user.Id);
            Console.Write("Initialising Pause Plan");
            if (mealkit != null)
            {
                if (mealkit.Status)
                {
                    mealkit.Status = false;
                    mealkit.userId = user.Id;
                    Console.WriteLine("Updated Meal Kit");

                    _mealKitService.UpdateMealKit(mealkit);
                    TempData["FlashMessage.Type"] = "success";
                    TempData["FlashMessage.Text"] = ("Your Meal Kit Plan Has Been Paused Successfully");
                    return Redirect("/user/CurrentMealKitPlan");
                }
                else
                {
                    mealkit.Status = true;
                    mealkit.userId = user.Id;
                    Console.WriteLine("Updated Meal Kit");

                    _mealKitService.UpdateMealKit(mealkit);
                    TempData["FlashMessage.Type"] = "success";
                    TempData["FlashMessage.Text"] = ("Your Meal Kit Plan Has Been Unpaused Successfully");
                    return Redirect("/user/CurrentMealKitPlan");
                }


            }

            return Page();
        }

        public async Task<IActionResult> OnPostCancelAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            MealKit? mealkit = _mealKitService.GetMealKitByUserId(user.Id);
            OrderDetails? orderDetails = _orderService.GetOrderDetailsByUserId(user.Id);
            
            if (mealkit != null)
            {
                var htmlPath = Path.Combine(_environment.ContentRootPath, "Pages/Templates/MealKitUnSubscriptionEmailTemplate.html");
                var subject = "Spoonful Meal Kit Unsubscription";
                string htmlBody = "";
                using (StreamReader streamReader = System.IO.File.OpenText(htmlPath))
                {
                    htmlBody = streamReader.ReadToEnd();
                }
                string messageBody = string.Format(htmlBody, user.UserName);
                // Call Email Service and send
                _emailSender.SendEmail(
                   user.Email,
                   subject,
                   messageBody,
                   null,
                   null);
                Console.WriteLine("Deleting Meal Kit");
                
                _mealKitService.DeleteMealKit(mealkit);
                _orderService.DeleteOrderDetails(orderDetails);
                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = ("Your Meal Kit Plan Has Been Cancelled Successfully.");
                return Redirect("/user/CurrentMealKitPlan");


            }

            return Page();
        }
        public async Task<IActionResult> OnPostSaveorderdetailsAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            OrderDetails? orderDetails = _orderService.GetOrderDetailsByUserId(user.Id);
            if (!ModelState.IsValid)
            {
                orderDetails.Address = MyOrderDetails.Address;
                orderDetails.OrderDate = MyOrderDetails.OrderDate;
                orderDetails.OrderTime = MyOrderDetails.OrderTime;
                orderDetails.AdditionalInstructions = MyOrderDetails.AdditionalInstructions;
                _orderService.UpdateOrderDetails(orderDetails);
                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = ("Your Order Details Has Been Saved Successfully.");
            }

            return Redirect("/user/CurrentMealKitPlan");
        }
    }
}
