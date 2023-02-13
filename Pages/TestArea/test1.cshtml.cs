using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IronPdf;
namespace Spoonful.Pages.TestArea
{
    public class test1Model : PageModel
    {
        public void OnGet()
        {
            //IronPDF generates pdf
            //var Renderer = new ChromePdfRenderer(); // Instantiates Chrome Renderer
            //var pdf = Renderer.RenderHtmlAsPdf(" <h1> ~Hello World~ </h1> Made with IronPDF!");
            //pdf.SaveAs("html_saved.pdf"); // Saves our PdfDocument object as a PDF
        }
    }
}
