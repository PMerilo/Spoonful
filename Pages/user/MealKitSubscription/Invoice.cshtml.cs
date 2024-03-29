using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.user.MealKitSubscription
{
    [Authorize]
    public class InvoiceModel : PageModel
    {
        private readonly UserManager<CustomerUser> _userManager;
        private readonly AuthDbContext _db;
        private readonly InvoiceMealKitService _invoiceMealKitService;
        private readonly MealKitService _mealKitService;
        public Invoice MyInvoice { get; set; }

        public InvoiceModel(UserManager<CustomerUser> userManager, AuthDbContext db, InvoiceMealKitService invoiceMealKitService, MealKitService mealKitService)
        {
            _userManager = userManager;
            _db = db;
            _invoiceMealKitService = invoiceMealKitService;
            _mealKitService = mealKitService;
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            MealKit? mealkit = _mealKitService.GetMealKitByUserId(user.Id);
            if(mealkit == null)
            {
                return Redirect("/user/CurrentMealKitPlan");
            }
            Invoice? invoice = _invoiceMealKitService.GetInvoiceByMealKitId(mealkit.Id);
            if (invoice != null)
            {
                MyInvoice = invoice;
                return Page();
                
            }
            return Redirect("/user/CurrentMealKitPlan");


        }
    }
}
