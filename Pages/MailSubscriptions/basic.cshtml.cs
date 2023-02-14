using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Spoonful.Models;
using Spoonful.Services;
using Stripe.Checkout;

namespace Spoonful.Pages.MailSubscriptions
{
    public class basicModel : PageModel
    {
        private readonly Spoonful.Models.AuthDbContext _context;
        private readonly UserManager<CustomerUser> userManager;
        public basicModel(Spoonful.Models.AuthDbContext context, UserManager<CustomerUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }
        [BindProperty]
        public MailSubscription mailsubscription { get; set; }
        public async Task<IActionResult> OnGet()
        {
            var user = await userManager.GetUserAsync(User);

            mailsubscription = await _context.mailsubsciption.FirstOrDefaultAsync(m => m.email == user.Email);
            if (mailsubscription == null)
            {
                var test = new MailSubscription()
                {
                    email = user.Email,
                    Subscriptiontype = "Basic",
                };
                _context.mailsubsciption.Add(test);
                await _context.SaveChangesAsync();
                return Redirect("/MailSubscriptions");
            }
            if (mailsubscription.Subscriptiontype != "Basic")
            {
                mailsubscription.Subscriptiontype = "Basic";
                mailsubscription.datetime = DateTime.Now.ToShortDateString();

                _context.mailsubsciption.Update(mailsubscription);
                await _context.SaveChangesAsync();
                return Redirect("/MailSubscriptions");


            }
            //error
            return Redirect("/MailSubscriptions");


        }
    }
}
