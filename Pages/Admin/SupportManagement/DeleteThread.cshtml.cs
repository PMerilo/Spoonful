using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Spoonful.Models;

namespace Spoonful.Pages.Admin.SupportManagement
{
    public class DeleteThreadModel : PageModel
    {
        private readonly Spoonful.Models.AuthDbContext _context;

        public DeleteThreadModel(Spoonful.Models.AuthDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public ProblemThread ProblemThread { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProblemThread = await _context.Problem.FirstOrDefaultAsync(m => m.Id == id);
            _context.Problem.Remove(ProblemThread);
            await _context.SaveChangesAsync();

            TempData["FlashMessage.Text"] = "Thread successfully deleted";
            TempData["FlashMessage.Type"] = "success";
            return Redirect("/Admin/SupportManagement/ProblemThreads");

        }
    }
}
