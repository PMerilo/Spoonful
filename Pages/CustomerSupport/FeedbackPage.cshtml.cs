using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Spoonful.Models;

namespace Spoonful.Pages.CustomerSupport
{
    public class CreateModel : PageModel
    {
        private readonly Spoonful.Models.AuthDbContext _context;
        private readonly UserManager<CustomerUser> userManager;


        public CreateModel(Spoonful.Models.AuthDbContext context, UserManager<CustomerUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Feedbackform Feedbackform { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(string cat)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                Feedbackform.Customername = user.UserName;
            }
            Feedbackform.Category = cat;
            _context.Feedback.Add(Feedbackform);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
            //change
        }
    }
}
