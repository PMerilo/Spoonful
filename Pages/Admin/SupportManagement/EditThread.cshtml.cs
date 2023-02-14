using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Microsoft.EntityFrameworkCore;
namespace Spoonful.Pages.Admin.SupportManagement
{
    public class EditThreadModel : PageModel
    {
        private readonly Spoonful.Models.AuthDbContext _context;

        public EditThreadModel(Spoonful.Models.AuthDbContext context)
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

            if (problem == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _context.Attach(problem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            TempData["FlashMessage.Text"] = "Edit successful";
            TempData["FlashMessage.Type"] = "success";


            return Redirect("/Admin/SupportManagement/ProblemThreads");
        }
    }
}
