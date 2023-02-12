using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Spoonful.Models;
using Spoonful.Services;
using Stripe.Checkout;

namespace Spoonful.Pages.MailSubscriptions
{
    public class IndexModel : PageModel
    {
        private readonly Spoonful.Models.AuthDbContext _context;
        private readonly UserManager<CustomerUser> userManager;
        public IndexModel(Spoonful.Models.AuthDbContext context, UserManager<CustomerUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }
        [BindProperty]
        public MailSubscription mailsubscription { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                var i = 1;

                mailsubscription = await _context.mailsubsciption.FirstOrDefaultAsync(m => m.email == user.Email);
                if (mailsubscription.Subscriptiontype == "Advanced") {
                    //error
                    return Redirect("/MailSubscriptions");
                }
                if (mailsubscription.Subscriptiontype == "Basic")
                {
                    i = 0;

                }
                var domain = "https://localhost:44367";
                var options = new SessionCreateOptions
                {
                    LineItems = new List<SessionLineItemOptions>
                    {
                        new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                UnitAmount = (long)1200,
                                Currency = "usd",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = "Spoonful Mail Subscription",
                                    Description = $"Advanced Plan," +
                                    $" starting on the {DateTime.Now.ToShortDateString()} "
                                },
                            },
                            Quantity = 1

                        },
                    },
                    PaymentMethodTypes = new List<string>
                    {
                        "card"
                    },
                    Mode = "payment",
                    SuccessUrl = domain + $"/user/MealKitSubscription/OrderConfirmed?id={user.Id}",
                    //SuccessUrl = domain + "/OrderConfirmed.cshtml?session_id={CHECKOUT_SESSION_ID}",
                    CancelUrl = domain + "/MailSubscriptions",
                };
                var service = new SessionService();
                Session session = service.Create(options);
                Response.Headers.Add("Location", session.Url);

                if (i == 0)
                {
                    mailsubscription.Subscriptiontype = "Advanced";
                    mailsubscription.datetime = DateTime.Now.ToShortDateString();
                    _context.mailsubsciption.Update(mailsubscription);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    mailsubscription.email = user.Email;
                    mailsubscription.Subscriptiontype = "Advanced";
                    _context.mailsubsciption.Add(mailsubscription);
                    await _context.SaveChangesAsync();
                }

                return new StatusCodeResult(303);

                //return Redirect("/user/MealKitSubscription/OrderConfirmed");
            }
            return Page();
            //change
        }
    }
}
