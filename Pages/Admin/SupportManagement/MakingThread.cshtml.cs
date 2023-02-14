using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Spoonful.Models;

namespace Spoonful.Pages.Admin.SupportManagement
{
    public class MakingThreadModel : PageModel
    {
        private readonly Spoonful.Models.AuthDbContext _context;

        public MakingThreadModel(Spoonful.Models.AuthDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public TixMod tix { get; set; }

        [BindProperty]
        public ProblemThread problemThread { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            tix = await _context.tired.FirstOrDefaultAsync(m => m.Id == id);
            if (tix == null)
            {
                return Redirect("/Admin/SupportManagement/TicketingManagement");
            }
            problemThread = await _context.Problem.FirstOrDefaultAsync(m => m.checker == id.ToString());
            if (problemThread != null)
            {
                TempData["FlashMessage.Text"] = $"Problem Thread for ticket #{id} already made";
                TempData["FlashMessage.Type"] = "danger";
                return Redirect("/Admin/SupportManagement/TicketingManagement");
            }
            var test = new ProblemThread()
            {
                username = tix.username,
                TitleFeedback = tix.TitleFeedback,
                MainFeedback = tix.MainFeedback,
                Category = tix.Category,
                datetime = tix.datetime,
                datetime2 = DateTime.Now.ToString(),
                answer = tix.answer,
                checker = tix.Id.ToString()

            };
            _context.Problem.Add(test);
            await _context.SaveChangesAsync();


            TempData["FlashMessage.Text"] = "Problem Thread successfully made";
            TempData["FlashMessage.Type"] = "success";
            return Redirect("/Admin/SupportManagement/TicketingManagement");

        }
    }
}
