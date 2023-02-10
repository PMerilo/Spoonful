using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Spoonful.Services;
using Stripe;

namespace Spoonful.Controllers
{
    [Route("/Admin/Invoices")]
    public class InvoiceController : Controller
    {
        private readonly InvoiceMealKitService _invoiceMealKitService;

        public InvoiceController(InvoiceMealKitService invoiceMealKitService)
        {
            _invoiceMealKitService = invoiceMealKitService;
        }

        [HttpGet]
        public async Task<ActionResult<String>> GenerateJsonData()
        {
            try
            {
                var invoices = _invoiceMealKitService.GetAll().GroupBy(x => new { group = x.DateOfPayment})
                    .Select(group => new
                    {
                        dateOfPayment = group.Key.group,
                        cost = group.Sum(x => x.Cost)
                    }).Reverse().ToList();



                return Ok(invoices);

            }
            catch (Exception)
            {
                Console.WriteLine("Error in generating chart data");
                return "error";
            }

        }
    }
}
