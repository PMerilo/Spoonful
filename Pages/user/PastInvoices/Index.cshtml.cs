using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;
using IronPdf;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Spoonful.Pages.user.PastInvoices
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly AuthDbContext _db;
        private readonly InvoiceMealKitService _invoiceMealKitService;
        private readonly UserManager<CustomerUser> _userManager;
        private IWebHostEnvironment _environment;
        public IEnumerable<Invoice> Invoices { get; set; }

        [BindProperty]
        public string InvoiceID { get; set; }

        public IndexModel(AuthDbContext db, InvoiceMealKitService invoiceMealKitService, UserManager<CustomerUser> userManager, IWebHostEnvironment environment)
        {
            _db = db;
            _invoiceMealKitService = invoiceMealKitService;
            _userManager = userManager;
            _environment = environment;

        }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            Invoices = _db.Invoice;
            Invoices = Invoices.Where(X => X.userId == user.Id).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Invoice? invoice = _invoiceMealKitService.GetInvoiceById(InvoiceID);

            if(invoice == null)
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = string.Format($"Error, PDF download not found : ID{InvoiceID}");
                return Redirect("/user/PastInvoices");
            }

            var filename = "Invoice Order ID -" + invoice.Id + ".pdf";
            var uploadsFolder = "pdfs";
            var pdfFile = Guid.NewGuid() + Path.GetExtension(filename);
            var pdfPath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder,filename);
            

            //IronPDF generates pdf
            var Renderer = new ChromePdfRenderer(); // Instantiates Chrome Renderer
            Renderer.RenderingOptions.CssMediaType = IronPdf.Rendering.PdfCssMediaType.Screen;
            Renderer.RenderingOptions.PrintHtmlBackgrounds = true;
            var pdf = Renderer.RenderHtmlAsPdf($" <h3> Invoice Order ID: {invoice.Id}</h3> Made with IronPDF!");
            pdf.SaveAs(pdfPath); // Saves our PdfDocument object as a PDF

            
            var result = File(pdf.Stream.ToArray(), "application/pdf");
            result.FileDownloadName = filename;
            return result;

            
        }
    }
}
