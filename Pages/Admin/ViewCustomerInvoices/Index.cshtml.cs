using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;
namespace Spoonful.Pages.Admin.ViewCustomerInvoices
{
    public class IndexModel : PageModel
    {
        private readonly AuthDbContext _db;
        private readonly InvoiceMealKitService _invoiceMealKitService;
        public IEnumerable<Invoice> Invoices { get; set; }
        public IndexModel(AuthDbContext db, InvoiceMealKitService invoiceMealKitService)
        {
            _db = db;
            _invoiceMealKitService = invoiceMealKitService;
        }

        public void OnGet()
        {
            Invoices = _db.Invoice;
        }
    }
}
