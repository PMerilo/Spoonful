using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Services;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.Aaron
{
    public class AddEntryModel : PageModel
    {
        [BindProperty]
        public Diary DiaryEntry { get; set; }

        private readonly DiaryService _diaryService;

        public AddEntryModel(DiaryService diaryService)
        {
            _diaryService = diaryService;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost() 
        {
            if (ModelState.IsValid)
            {
                var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                var dateValue = DateTime.Compare(DiaryEntry.Purchase, currentDate);
                if (dateValue > 0)
                {
                    //Invalid date
                    //log , show error
                    ModelState.AddModelError("DiaryEntry.Purchase", "Purchase Date exists in the future.");
                    return Page();
                }
                _diaryService.AddEntry(DiaryEntry);
                TempData["FlashMessage.Type"] = "success";
                return Redirect("/Aaron/Grocery%20Tracker/Index");
            }
            return Page();
        }
    }
}
