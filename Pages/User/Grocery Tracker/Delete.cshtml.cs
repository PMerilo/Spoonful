using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Services;
using Spoonful.Models;

namespace Spoonful.Pages.Aaron
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public Diary DiaryEntry { get; set; }
        [BindProperty]
        public string userIdvar { get; set; }

        private readonly DiaryService _diaryService;

        public DeleteModel(DiaryService diaryService)
        {
            _diaryService = diaryService;
        }
        public IActionResult OnGet(int id)
        {
            Diary? entry = _diaryService.GetEntryById(id);
            if (entry != null)
            {
                DiaryEntry = entry;
                userIdvar = entry.userId;
                _diaryService.DeleteEntry(entry);
                string userUrl = "/User/Grocery%20Tracker/Index?id=" + userIdvar;
                return Redirect(userUrl);
            }
            else
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = string.Format("Entry ID {0} not found", id);
                string userUrl = "/User/Grocery%20Tracker/Index?id=" + userIdvar;
                return Redirect(userUrl);
            }
        }
    }
}
