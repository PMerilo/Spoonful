using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.Admin.VoucherManagement
{
    public class EditModel : PageModel
    {
        private readonly VoucherService _voucherService;


        public EditModel(VoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        [BindProperty]
        public Vouchers Voucher { get; set; } = new();
        public IActionResult OnGet(int id)
        {
            Vouchers? voucher = _voucherService.GetVoucherById(id);
            if (voucher != null)
            {
                Voucher = voucher;
                return Page();
            }
            else
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = string.Format("Voucher ID {0} not found", id);
                return Redirect("/Admin/VoucherManagement");
            }
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _voucherService.UpdateVoucher(Voucher);
                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = string.Format("Voucher {0} is updated", Voucher.Id);
                return Redirect("/Admin/VoucherManagement");
            }
            return Page();
        }
    }
}
