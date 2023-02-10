using Microsoft.AspNetCore.Mvc;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Controllers
{
    [Route("/Admin/charts/usedVouchers")]
    public class RedeemedVoucherController : Controller
    {
        private readonly VoucherService _voucherService;

        public RedeemedVoucherController(VoucherService voucherService)
        {
            _voucherService = voucherService;
        }


        [HttpGet]
        public async Task<ActionResult<String>> GenerateJsonData()
        {
            try
            {
                var usedVouchers = _voucherService.GetAll().Select(voucher => new
                    {
                        usedVouchers = voucher.Name,
                        used = voucher.Used
                    }).ToList();



                return Ok(usedVouchers);

            }
            catch (Exception)
            {
                Console.WriteLine("Error in generating chart data");
                return "error";
            }

        }

    }
}
