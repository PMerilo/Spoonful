using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.Admin.VoucherManagement
{
    public class SentEmailsModel : PageModel
    {
        private readonly VoucherEmailService _voucherEmailService;

        public SentEmailsModel(VoucherEmailService voucherEmailService)
        {
            _voucherEmailService = voucherEmailService;
        }

        public List<VoucherEmails> emailList { get; set; } = new();
        public void OnGet()
        {
            emailList = _voucherEmailService.GetAll();
        }
    }
}
