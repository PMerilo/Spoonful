using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Services;
using Spoonful.Models;

namespace Spoonful.Pages.Aaron
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public Diary DiaryEntry { get; set; }

        private readonly DiaryService _diaryService;

        public EditModel(DiaryService diaryService)
        {
            _diaryService = diaryService;
        }
        public IActionResult OnGet(int id)
        {

            Diary? entry = _diaryService.GetEntryById(id);
            if (entry != null)
            {
                DiaryEntry = entry;
                return Page();
            }
            else
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = string.Format("Entry ID {0} not found", id);
                return Redirect("/Aaron/Grocery%20Tracker/Index");
            }
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
                    ModelState.AddModelError("", "Purchase Date exists in the future.");
                    return Page();
                }

                _diaryService.UpdateEntry(DiaryEntry);
                TempData["FlashMessage.Type"] = "success";
                return Redirect("/Aaron/Grocery%20Tracker/Index");
            }
            return Page();
        }

    }
}
