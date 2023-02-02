using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;


namespace Spoonful.Pages.Admin.VoucherManagement
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

        public IActionResult OnPost()
        {
            Voucher = _voucherService.GetVoucherById(Voucher.Id);
            if (Voucher != null)
            {
                _voucherService.DeleteVoucher(Voucher);
                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = string.Format("Voucher {0} is updated", Voucher.Id);
            }
            return Redirect("/Admin/VoucherManagement");
        }
    }
}
