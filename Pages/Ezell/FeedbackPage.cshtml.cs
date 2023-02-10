using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Spoonful.Models;

namespace Spoonful.Pages.Ezell
{
    public class CreateModel : PageModel
    {
        private readonly Spoonful.Models.AuthDbContext _context;

        public CreateModel(Spoonful.Models.AuthDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Feedbackform Feedbackform { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _context.Feedback.Add(Feedbackform);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
