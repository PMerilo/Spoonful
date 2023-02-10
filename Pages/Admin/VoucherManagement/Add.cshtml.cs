using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.Admin.VoucherManagement
{
    public class AddModel : PageModel
    {
        private readonly VoucherService _voucherService;
        private IWebHostEnvironment _environment;


        public AddModel(VoucherService voucherService, IWebHostEnvironment environment)
        {
            _voucherService = voucherService;
            _environment = environment;
        }

        [BindProperty]
        public string Name { get; set; }
        [BindProperty]
        public string Description { get; set; }
        [BindProperty]
        public string voucherCode { get; set; }
        [BindProperty]
        public int discountAmount { get; set; }
        [BindProperty]
        public int Quantity { get; set; }
        [BindProperty]
        public int Used { get; set; }
        [BindProperty]
        public DateTime expiryDate { get; set; }
        [BindProperty]
        public IFormFile? Upload { get; set; }


        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Vouchers Voucher = new();
            if (ModelState.IsValid)
            {
                Vouchers? voucher = _voucherService.GetVoucherByCode(voucherCode);
                if (voucher != null)
                {
                    ModelState.AddModelError("Voucher.voucherCode", "Voucher Code alreay exists.");
                    return Page();
                }
                if (Upload != null)
                {
                    if (Upload.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("Upload",
                        "File size cannot exceed 2MB.");
                        return Page();
                    }
                    var uploadsFolder = "uploads/rewardsUpload";
                    var imageFile = Guid.NewGuid() + Path.GetExtension(
                    Upload.FileName);
                    var imagePath = Path.Combine(_environment.ContentRootPath,
                    "wwwroot", uploadsFolder, imageFile);
                    using var fileStream = new FileStream(imagePath,FileMode.Create);
                    await Upload.CopyToAsync(fileStream);
                    Voucher.ImageURL = string.Format("/{0}/{1}", uploadsFolder,imageFile);
                }
                Voucher.Name = Name;
                Voucher.Description = Description;
                Voucher.voucherCode = voucherCode;
                Voucher.discountAmount = discountAmount;
                Voucher.Quantity = Quantity;
                Voucher.Used = 0;
                Voucher.expiryDate = expiryDate;

                _voucherService.AddVoucher(Voucher);
                TempData["FlashMessage.Type"] = "success";
                /*TempData["FlashMessage.Text"] = string.Format("Vouchers {0} is added", MyEmployee.Name);*/
                return Redirect("/Admin/VoucherManagement");
            }
            TempData["FlashMessage.Type"] = "error";
            TempData["FlashMessage.Text"] = "Modal failed";
            return Page();
        }
    }
}
