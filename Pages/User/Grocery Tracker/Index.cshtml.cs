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

        [BindProperty]
        public string hideFilter { get; set; }

        [BindProperty]
        public string userIdvar { get; set; }

        private readonly DiaryService _diaryService;

        public IndexModel(DiaryService diaryService)
        {
            _diaryService = diaryService;
        }

        public List<Diary> DiaryList { get; set; } = new();

        public void OnGet(string id)
        {
            userIdvar = id;
            
            DiaryList = _diaryService.GetAllByCat(id);
        }

        public IActionResult OnPost() 
        {
            switch(sortFilter)
            {
                case "name":
                    DiaryList = _diaryService.GetAllByName(userIdvar);
                    break;

                case "category":
                    DiaryList = _diaryService.GetAllByCat(userIdvar);
                    break;

                case "purchase":
                    DiaryList = _diaryService.GetAllByPurchase(userIdvar);
                    break;

                case "expiry":
                    DiaryList = _diaryService.GetAllByExpiry(userIdvar);
                    break;

                default:
                    DiaryList = _diaryService.GetAll(userIdvar);
                    break;
            }

            switch (hideFilter)
            {

                case "seafood":
                    DiaryList = _diaryService.GetAllInCategory(userIdvar, "seafood");
                    break;

                case "meat":
                    DiaryList = _diaryService.GetAllInCategory(userIdvar, "meat");
                    break;

                case "grain":
                    DiaryList = _diaryService.GetAllInCategory(userIdvar, "grain");
                    break;

                case "today expiry":
                    DiaryList = _diaryService.GetTodayExpire(userIdvar, DateTime.Today);
                    break;

                default:
                    DiaryList = _diaryService.GetAll(userIdvar);
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
