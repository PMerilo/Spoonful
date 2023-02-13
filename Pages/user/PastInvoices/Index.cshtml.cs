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
            var htmlPath = Path.Combine(_environment.ContentRootPath, "Pages/Templates/Invoice.html");
            var cssPath = Path.Combine(_environment.ContentRootPath, "wwwroot/css/admin.css");
            string HtmlBody = "";

            using (StreamReader streamReader = System.IO.File.OpenText(htmlPath))
            {
                HtmlBody = streamReader.ReadToEnd();
            }

            // {0} Name
            // {1} Address
            // {2} Email
            // {3} Date Of Payment
            // {4} Menu Preference (Current Plan)
            // {5} No Of Recipes Per Week
            // {6} No Of People Per Week
            // {7} No Of Servings Per Person
            // {8} Invoice Cost

           
            string messageBody = string.Format(HtmlBody,
                invoice.Name,
                invoice.Address,
                invoice.Name,
                invoice.Address,
                invoice.Email,
                invoice.DateOfPayment,
                invoice.MenuPreference,
                invoice.noOfPeoplePerWeek,
                invoice.noOfPeoplePerWeek,
                invoice.noOfServingsPerPerson,
                invoice.Cost,
                invoice.Cost,
                invoice.Cost,
                invoice.Cost
                );

            //IronPDF generates pdf
            //var Renderer = new ChromePdfRenderer(); // Instantiates Chrome Renderer
            //var Renderer = new HtmlToPdf();
            var Renderer = new IronPdf.ChromePdfRenderer();
            Renderer.RenderingOptions.PaperSize = IronPdf.Rendering.PdfPaperSize.A2;
            
            Renderer.RenderingOptions.CssMediaType = IronPdf.Rendering.PdfCssMediaType.Screen;
            //Renderer.RenderingOptions.PrintHtmlBackgrounds = true;

            var pdf = Renderer.RenderHtmlAsPdf(messageBody);
            //css path not working fix it tmr
            //var pdf = Renderer.RenderHtmlAsPdf(messageBody, cssPath);
            //var pdf = Renderer.RenderHtmlFileAsPdf(htmlPath);

            //var pdf = Renderer.RenderHtmlAsPdf();

            pdf.SaveAs(pdfPath); // Saves our PdfDocument object as a PDF

            // To Show the download link in the browser
            var result = File(pdf.Stream.ToArray(), "application/pdf");
            // To name the file of the downloaded pdf
            result.FileDownloadName = filename;
            return result;

            
        }
    }
}
