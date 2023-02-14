using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Spoonful.Models;
using Spoonful.Services;


namespace Spoonful.Pages.CustomerSupport
{
    public class ApplyTicketingModel : PageModel
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<CustomerUser> userManager;
        private readonly TicketingService _ticketservice;

        public ApplyTicketingModel(AuthDbContext context, UserManager<CustomerUser> userManager, TicketingService ticketservice)
        {
            _context = context;
            this.userManager = userManager;
            _ticketservice = ticketservice;
        }
        [BindProperty]
        public TixMod Ticketings { get; set; }
        public IList<TixMod> tixMods { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await userManager.GetUserAsync(User);
            Ticketings = await _context.tired.FirstOrDefaultAsync(m => m.email == user.Email);
            if (Ticketings != null)
            {
                tixMods = await _context.tired.ToListAsync();
                foreach (var item in tixMods)
                {
                    if (item.email == user.Email)
                    {
                        if (item.Feedbackstatus != "Resolved")
                        {
                            string userUrl = "/CustomerSupport/EndTicket?id=" + item.Id;
                            return Redirect(userUrl);
                        }
                    }
                }
            }
            return Page();
        }



        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            

            _context.tired.Add(Ticketings);
            var user = await userManager.GetUserAsync(User);
            Ticketings = await _context.tired.FirstOrDefaultAsync(m => m.email == user.Email);
            if (Ticketings != null)
            {
                tixMods = await _context.tired.ToListAsync();
                foreach (var item in tixMods)
                {
                    if(item.email == user.Email)
                    {
                        if (item.Feedbackstatus != "Resolved")
                        {
                            TempData["FlashMessage.Text"] = "You can only have one ongoing ticket at a time.";
                            TempData["FlashMessage.Type"] = "danger";
                            return Page();
                        }
                    }
                }
            }

            if (user != null)
            {
                await _context.SaveChangesAsync();
                TempData["FlashMessage.Text"] = "Ticket created successfully";
                TempData["FlashMessage.Type"] = "success";
                return Redirect("/Index");
            }
            return Page();
        }

    }
}
