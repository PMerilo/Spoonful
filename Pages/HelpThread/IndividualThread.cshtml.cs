using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.HelpThread
{
    public class IndividualThreadModel : PageModel
    {
        private readonly Spoonful.Models.AuthDbContext _context;

        public IndividualThreadModel(Spoonful.Models.AuthDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public ProblemThread problem { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            problem = await _context.Problem.FirstOrDefaultAsync(m => m.Id == id);
            return Page();
        }
    }
}
