using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IronPdf;
namespace Spoonful.Pages.TestArea
{
    public class test1Model : PageModel
    {
        public void OnGet()
        {

            var Renderer = new ChromePdfRenderer(); // Instantiates Chrome Renderer

            // To include elements that are usually removed to save ink during printing we choose screen
            Renderer.RenderingOptions.CssMediaType = IronPdf.Rendering.PdfCssMediaType.Screen;

            var pdf = Renderer.RenderUrlAsPdf("https://getbootstrap.com/");
            pdf.SaveAs("url_saved.pdf");
        }
    }
}
