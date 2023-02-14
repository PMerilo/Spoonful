using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Spoonful.Models;

namespace Spoonful.Pages.Admin.SupportManagement
{
    public class TicketingManagementModel : PageModel
    {
        private readonly Spoonful.Models.AuthDbContext _context;

        public TicketingManagementModel(Spoonful.Models.AuthDbContext context)
        {
            _context = context;
        }

        public IList<TixMod> tixMods { get; set; }

        public async Task OnGetAsync()
        {
            tixMods = await _context.tired.ToListAsync();
        }


    }
}
