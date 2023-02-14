using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Spoonful.Models;

namespace Spoonful.Pages.CustomerSupport
{
    public class EditModel : PageModel
    {
        private readonly Spoonful.Models.AuthDbContext _context;

        public EditModel(Spoonful.Models.AuthDbContext context)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _context.Attach(Feedbackform).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeedbackformExists(Feedbackform.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool FeedbackformExists(Guid id)
        {
            return _context.Feedback.Any(e => e.Id == id);
        }
    }
}
