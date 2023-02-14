using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Spoonful.Models;

namespace Spoonful.Pages.Admin.SupportManagement
{
    public class IndexModel : PageModel
    {
        private readonly Spoonful.Models.AuthDbContext _context;

        public IndexModel(Spoonful.Models.AuthDbContext context)
        {
            _context = context;
        }

        public IList<Feedbackform> Feedbackform { get; set; }

        public async Task OnGetAsync()
        {
            Feedbackform = await _context.Feedback.ToListAsync();
        }
    }
}
