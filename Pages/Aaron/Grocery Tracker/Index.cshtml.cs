using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.Aaron
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public Diary DiaryEntry { get; set; }

        [BindProperty]
        public string sortFilter { get; set; }

        private readonly DiaryService _diaryService;

        public IndexModel(DiaryService diaryService)
        {
            _diaryService = diaryService;
        }

        public List<Diary> DiaryList { get; set; } = new();

        public void OnGet()
        {
            DiaryList = _diaryService.GetAllByCat();
        }

        public IActionResult OnPost() 
        {
            switch(sortFilter)
            {
                case "name":
                    DiaryList = _diaryService.GetAllByName();
                    break;

                case "category":
                    DiaryList = _diaryService.GetAllByCat();
                    break;

                case "purchase":
                    DiaryList = _diaryService.GetAllByPurchase();
                    break;

                case "expiry":
                    DiaryList = _diaryService.GetAllByExpiry();
                    break;

                default:
                    DiaryList = _diaryService.GetAll();
                    break;
            }

            return Page();
        }

        public IActionResult OnDeletePost(int id)
        {
            Diary? entry = _diaryService.GetEntryById(id);
            if (entry != null)
            {
                DiaryEntry = entry;
                _diaryService.DeleteEntry(entry);
                return Redirect("/Aaron/Grocery%20Tracker/Index");
            }
            else
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = string.Format("Entry ID {0} not found", id);
                return Redirect("/Aaron/Grocery%20Tracker/Index");
            }
        }
    }
}
