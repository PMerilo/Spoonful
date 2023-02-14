using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Spoonful.Models;

namespace Spoonful.Pages.CustomerSupport
{
    public class ForceDeleteModel : PageModel
    {
        private readonly UserManager<CustomerUser> userManager;

        private readonly Spoonful.Models.AuthDbContext _context;

        public ForceDeleteModel(Spoonful.Models.AuthDbContext context, UserManager<CustomerUser> userManager)
        {
            this.userManager = userManager;
            _context = context;
        }
        [BindProperty]
        public TixMod Ticketings { get; set; }
        public IList<TixMod> tixMods { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

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
                            _context.tired.Remove(item);
                            await _context.SaveChangesAsync();
                            
                        }
                    }
                }
            }
            return Redirect("/Index");

        }
    }
}
