using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Spoonful.Models;

namespace Spoonful.Pages.CustomerSupport
{
    public class EndTicketModel : PageModel
    {
        private readonly Spoonful.Models.AuthDbContext _context;

        public EndTicketModel(Spoonful.Models.AuthDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public TixMod Ticketings { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Ticketings = await _context.tired.FirstOrDefaultAsync(m => m.Id == id);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int id2)
        {
            Ticketings = await _context.tired.FirstOrDefaultAsync(m => m.Id == id2);
            Ticketings.Feedbackstatus = "Resolved";
            _context.tired.Update(Ticketings);
            await _context.SaveChangesAsync();
            TempData["FlashMessage.Text"] = $"Ticket resolved!";
            TempData["FlashMessage.Type"] = "success";

            return Redirect("/Index");
        }

    }
}
