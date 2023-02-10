using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.Admin.ViewCustomerInvoices
{
    public class CustomerInvoiceModel : PageModel
    {
        private readonly UserManager<CustomerUser> _userManager;
        private readonly AuthDbContext _db;
        private readonly InvoiceMealKitService _invoiceMealKitService;
        private readonly MealKitService _mealKitService;

        public CustomerInvoiceModel(UserManager<CustomerUser> userManager, AuthDbContext db, InvoiceMealKitService invoiceMealKitService, MealKitService mealKitService)
        {
            _userManager = userManager;
            _db = db;
            _invoiceMealKitService = invoiceMealKitService;
            _mealKitService = mealKitService;
        }

        public Invoice MyInvoice { get; set; }

        public async Task<IActionResult> OnGet(string id)
        {
            //var user = await _userManager.GetUserAsync(User);
            MealKit? mealkit = _mealKitService.GetMealKitByUserId(id);
            if (mealkit == null)
            {
                return Redirect("/Admin/ViewCustomerInvoices/CustomerInvoice");
            }
            Invoice? invoice = _invoiceMealKitService.GetInvoiceByMealKitId(mealkit.Id);
            if (invoice != null)
            {
                MyInvoice = invoice;
                return Page();

            }
            return Redirect("/Admin/ViewCustomerInvoices");
        }
    }
}
