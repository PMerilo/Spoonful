using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Spoonful.Models;

namespace Spoonful.Pages.Admin.SupportManagement
{
    public class DeleteTicketModel : PageModel
    {
        private readonly Spoonful.Models.AuthDbContext _context;

        public DeleteTicketModel(Spoonful.Models.AuthDbContext context)
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
            _context.tired.Remove(tix);
            await _context.SaveChangesAsync();

            TempData["FlashMessage.Text"] = "Ticket successfully deleted";
            TempData["FlashMessage.Type"] = "success";
            return Redirect("/Admin/SupportManagement/Spam");

        }
    }
}
