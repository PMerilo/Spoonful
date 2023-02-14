using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.Admin.SupportManagement
{
    public class SendMailModel : PageModel
    {
        public readonly IEmailService _emailSender;
        private readonly Spoonful.Models.AuthDbContext _context;

        public SendMailModel(Spoonful.Models.AuthDbContext context, IEmailService emailSender)
        {
            _emailSender = emailSender;

            _context = context;
        }
        [BindProperty]
        public MailSubscription Mail { get; set; }
        public IList<MailSubscription> test { get; set; }

        public async Task<IActionResult> OnPostAsync(string cat, string subject, string message)
        {
            test = await _context.mailsubsciption.ToListAsync();
            if (test == null)
            {
                return Page();
            }
            foreach (var mail in test)
            {
                if (cat == mail.Subscriptiontype)
                {
                    var result = _emailSender.SendEmail(
                    mail.email,
                    $"{subject}",
                    $"{message}",
                    null,
                    null);
                    TempData["FlashMessage.Text"] = $"Email successfully sent.";
                    TempData["FlashMessage.Type"] = "success";
                }
                
            }
            
            
            return Page();
        }
    }
}
