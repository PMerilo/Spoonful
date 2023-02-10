using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.user.PastInvoices
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly AuthDbContext _db;
        private readonly InvoiceMealKitService _invoiceMealKitService;
        private readonly UserManager<CustomerUser> _userManager;
        public IEnumerable<Invoice> Invoices { get; set; }

        public IndexModel(AuthDbContext db, InvoiceMealKitService invoiceMealKitService, UserManager<CustomerUser> userManager)
        {
            _db = db;
            _invoiceMealKitService = invoiceMealKitService;
            _userManager = userManager;
            
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            Invoices = _db.Invoice;
            Invoices = Invoices.Where(X=> X.userId == user.Id).ToList();

            return Page();
        }
    }
}
