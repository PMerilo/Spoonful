using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Spoonful.Models;

namespace Spoonful.Pages.MailSubscriptions
{
    public class OrderConfirmedModel : PageModel
    {
        private readonly Spoonful.Models.AuthDbContext _context;
        private readonly UserManager<CustomerUser> userManager;
        public OrderConfirmedModel(Spoonful.Models.AuthDbContext context, UserManager<CustomerUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }
        [BindProperty]
        public MailSubscription mailsubscription { get; set; }


        public async Task<IActionResult> OnGet(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }
            else
            {
                var user = await userManager.GetUserAsync(User);

                if (user != null)
                {
                    mailsubscription = await _context.mailsubsciption.FirstOrDefaultAsync(m => m.email == user.Email);
                    if (mailsubscription != null)
                    {
                        mailsubscription.Subscriptiontype = "Advanced";
                        mailsubscription.datetime = DateTime.Now.ToShortDateString();
                        _context.mailsubsciption.Update(mailsubscription);
                        await _context.SaveChangesAsync();
                    }
                    
                }
                else
                {
                    mailsubscription.email = user.Email;
                    mailsubscription.Subscriptiontype = "Advanced";
                    _context.mailsubsciption.Add(mailsubscription);
                    await _context.SaveChangesAsync();
                }
                    return Page();
            }

        }
    }
}
