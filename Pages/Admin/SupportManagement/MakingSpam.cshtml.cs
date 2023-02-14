using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Spoonful.Models;
namespace Spoonful.Pages.Admin.SupportManagement
{
    public class MakingSpamModel : PageModel
    {
        private readonly Spoonful.Models.AuthDbContext _context;

        public MakingSpamModel(Spoonful.Models.AuthDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public TixMod tix { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            tix = await _context.tired.FirstOrDefaultAsync(m => m.Id == id);

            tix.Feedbackstatus = "Spam";
            _context.Attach(tix).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            TempData["FlashMessage.Text"] = "Ticket successfully marked as spam";
            TempData["FlashMessage.Type"] = "success";
            return Redirect("/Admin/SupportManagement/TicketingManagement");

        }

    }
}
