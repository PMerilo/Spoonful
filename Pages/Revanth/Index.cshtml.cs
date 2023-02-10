using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.Revanth
{
    public class IndexModel : PageModel
    {
        private readonly VoucherService _voucherService;

        public IndexModel(VoucherService voucherService)
        {
            _voucherService = voucherService;
        }
        [BindProperty]
        public Vouchers Voucher { get; set; } = new();
        public List<Vouchers> VoucherList { get; set; } = new();

        public void OnGet()
        {
            VoucherList = _voucherService.GetAll();
        }
    }
}
