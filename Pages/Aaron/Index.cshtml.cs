using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.Aaron
{
    public class IndexModel : PageModel
    {
        private readonly DiaryService _diaryService;

        public IndexModel(DiaryService diaryService)
        {
            _diaryService = diaryService;
        }

        public List<Diary> DiaryList { get; set; } = new();

        public void OnGet()
        {
            DiaryList = _diaryService.GetAll();
        }
    }
}
}
