using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Spoonful.Models;

namespace Spoonful.Pages.HelpThread
{
    public class IndexModel : PageModel
    {
        private readonly Spoonful.Models.AuthDbContext _context;

        public IndexModel(Spoonful.Models.AuthDbContext context)
        {
            _context = context;
        }

        public IList<ProblemThread> problem { get; set; }

        public async Task OnGetAsync()
        {
            problem = await _context.Problem.ToListAsync();
        }
    }
}
