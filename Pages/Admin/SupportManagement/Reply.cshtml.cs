using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.Admin.SupportManagement
{
    public class ReplyModel : PageModel
    {
        public readonly IEmailService _emailSender;
        private readonly Spoonful.Models.AuthDbContext _context;

        public ReplyModel(Spoonful.Models.AuthDbContext context, IEmailService emailSender)
        {
            _emailSender = emailSender;

            _context = context;
        }
        [BindProperty]
        public TixMod TixMod { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            TixMod = await _context.tired.FirstOrDefaultAsync(m => m.Id == id);
            TixMod.Feedbackstatus = "Ongoing";
            _context.tired.Update(TixMod);
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string answer, int id1)
        {
            TixMod = await _context.tired.FirstOrDefaultAsync(m => m.Id == id1);
            TixMod.answer = answer;
            _context.tired.Update(TixMod);
            await _context.SaveChangesAsync();


            if(TixMod.notifypreference == "email")
            {
                var result = _emailSender.SendEmail(
                TixMod.email,
                "Admin Response",
                $"{TixMod.answer}" +
                $"If this resolves your problem, simply end your ticket by going to our website > log in > Services > Customer Service",
                null,
                null);
                TempData["FlashMessage.Text"] = $"Email successfully sent.";
                TempData["FlashMessage.Type"] = "success";

                if (!result)
                {
                    TempData["FlashMessage.Text"] = $"Failed to send email.";
                    TempData["FlashMessage.Type"] = "danger";
                }
            }

            return Redirect("/Admin/SupportManagement/TicketingManagement");



        }


    }
}
