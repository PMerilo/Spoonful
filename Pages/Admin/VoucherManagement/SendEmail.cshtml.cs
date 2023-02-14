using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Spoonful.Pages.Admin.VoucherManagement
{
    public class SendEmailModel : PageModel
    {

        private readonly VoucherService _voucherService;
        private readonly UserManager<CustomerUser> userManager;
        private readonly VoucherEmailService _voucherEmailService;
        private readonly AuthDbContext _db;
        public readonly IEmailService _emailSender;

        public SendEmailModel(VoucherService voucherService, VoucherEmailService voucherEmailService, IEmailService emailSender, UserManager<CustomerUser> userManager, AuthDbContext db)
        {
            _voucherService = voucherService;
            _voucherEmailService = voucherEmailService;
            _emailSender = emailSender;
            this.userManager = userManager;
            _db = db;
        }
        [BindProperty]
        public Vouchers Voucher { get; set; } = new();
        [BindProperty]
        public VoucherEmails Email { get; set; } = new();
        public List<CustomerDetails> userList { get; set; } = new();

        public string email { get; set; }

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

        public IActionResult OnPost(int id)
        {
            Email.vouchersId = id;
            if (Email.sendTo == "all")
            {
                userList = _db.CustomerDetails.Include(u => u.User).ToList();
                foreach (var user in userList)
                {
                    email = user.User.Email;
                    var result = _emailSender.SendEmail(
                        email,
                        Email.Subject,
                        Email.HtmlContent,
                        null,
                        null);
                    if (!result)
                    {
                        TempData["FlashMessage.Text"] = $"Failed to send email.";
                        TempData["FlashMessage.Type"] = "danger";
                    }
                }
            }
            else
            {
                userList = _db.CustomerDetails.Include(u => u.User).ToList();
                foreach (var user in userList)
                {
                    email = user.User.Email;
                    var result = _emailSender.SendEmail(
                        email,
                        Email.Subject,
                        Email.HtmlContent,
                        null,
                        null);
                    if (!result)
                    {
                        TempData["FlashMessage.Text"] = $"Failed to send email.";
                        TempData["FlashMessage.Type"] = "danger";
                    }
                }
            }

            _voucherEmailService.AddVoucherEmail(Email);
            TempData["FlashMessage.Type"] = "success";
            /*TempData["FlashMessage.Text"] = string.Format("Vouchers {0} is added", MyEmployee.Name);*/
            return Redirect("/Admin/VoucherManagement");
        }
    }
}
