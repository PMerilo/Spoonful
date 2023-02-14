using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.Admin.SupportManagement
{
    public class ProblemThreadsModel : PageModel
    {
        private readonly Spoonful.Models.AuthDbContext _context;

        public ProblemThreadsModel(Spoonful.Models.AuthDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public ProblemThread ProblemThread { get; set; } = new();
        public IList<ProblemThread> problem { get; set; }

        public async Task OnGetAsync()
        {
            problem = await _context.Problem.ToListAsync();
        }
       
    }
}
