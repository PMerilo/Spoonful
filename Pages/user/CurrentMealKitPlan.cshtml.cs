using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace Spoonful.Pages.user
{
    [BindProperties]
    public class CurrentMealKitPlanModel : PageModel
    {
        private readonly AuthDbContext _db;

        private readonly MealKitService _mealKitService;

        private readonly UserManager<CustomerUser> _userManager;

        private readonly OrderService _orderService;

        private readonly InvoiceMealKitService _invoiceMealKitService;


        public CurrentMealKitPlanModel(AuthDbContext db, MealKitService mealKitService, UserManager<CustomerUser> userManager, OrderService orderService, InvoiceMealKitService invoiceMealKitService)
        {
            _db = db;
            _mealKitService = mealKitService;
            _userManager = userManager;
            _orderService = orderService;
            _invoiceMealKitService = invoiceMealKitService;
        }

        [BindProperty]
        public MealKit MyMealKit { get; set; }


        public OrderDetails MyOrderDetails { get; set; }




        public async Task<IActionResult> OnGet()
        {

            var user = await _userManager.GetUserAsync(User);
            MealKit? mealkit = _mealKitService.GetMealKitByUserId(user.Id);
            OrderDetails? orderDetails = _orderService.GetOrderDetailsByUserId(user.Id);
            
            if (mealkit != null)
            {

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
                mealkit.noOfPeoplePerWeek = MyMealKit.noOfPeoplePerWeek;
                mealkit.MenuPreference = MyMealKit.MenuPreference;
                mealkit.noOfServingsPerPerson = MyMealKit.noOfServingsPerPerson;
                mealkit.noOfRecipesPerWeek = MyMealKit.noOfRecipesPerWeek;
                mealkit.userId = user.Id;

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
            
            return Page();
        }
    }
}
