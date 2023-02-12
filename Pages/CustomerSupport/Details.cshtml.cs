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
    public class DetailsModel : PageModel
    {
        private readonly Spoonful.Models.AuthDbContext _context;

        public DetailsModel(Spoonful.Models.AuthDbContext context)
        {
            _context = context;
        }

        public Feedbackform Feedbackform { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Feedbackform = await _context.Feedback.FirstOrDefaultAsync(m => m.Id == id);

            Feedbackform.Feedbackstatus = "Seen";
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

            if (Feedbackform == null)
            {
                return NotFound();
            }
            return Page();
        }
        private bool FeedbackformExists(Guid id)
        {
            return _context.Feedback.Any(e => e.Id == id);
        }
    }
}
