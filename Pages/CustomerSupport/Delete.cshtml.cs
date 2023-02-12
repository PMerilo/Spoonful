using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Spoonful.Models;

namespace Spoonful.Pages.CustomerSupport
{
    public class DeleteModel : PageModel
    {
        private readonly Spoonful.Models.AuthDbContext _context;

        public DeleteModel(Spoonful.Models.AuthDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Feedbackform Feedbackform { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Feedbackform = await _context.Feedback.FirstOrDefaultAsync(m => m.Id == id);

            if (Feedbackform == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Feedbackform = await _context.Feedback.FindAsync(id);

            if (Feedbackform != null)
            {
                _context.Feedback.Remove(Feedbackform);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
