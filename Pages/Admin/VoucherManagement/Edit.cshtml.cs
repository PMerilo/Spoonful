using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.Admin.VoucherManagement
{
    public class EditModel : PageModel
    {
        private readonly VoucherService _voucherService;
        private IWebHostEnvironment _environment;

        public EditModel(VoucherService voucherService, IWebHostEnvironment environment)
        {
            _voucherService = voucherService;
            _environment = environment;
        }

        [BindProperty]
        public Vouchers Voucher { get; set; } = new();
        [BindProperty]
        public IFormFile? Upload { get; set; }
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

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                if (Upload != null)
                {
                    if (Upload.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("Upload", "File size cannot exceed 2MB.");
                        return Page();
                    }

                    var uploadsFolder = "uploads/rewardsUpload";
                    if (Voucher.ImageURL != null)
                    {
                        var oldImageFile = Path.GetFileName(Voucher.ImageURL);
                        var oldImagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, oldImageFile);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    var imageFile = Guid.NewGuid() + Path.GetExtension(Upload.FileName);
                    var imagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, imageFile);
                    using var fileStream = new FileStream(imagePath, FileMode.Create);
                    await Upload.CopyToAsync(fileStream);
                    Voucher.ImageURL = string.Format("/{0}/{1}", uploadsFolder, imageFile);
                }
                _voucherService.UpdateVoucher(Voucher);
                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = string.Format("Voucher {0} is updated", Voucher.Id);
                return Redirect("/Admin/VoucherManagement");
            }
            return Page();
        }
    }
}
