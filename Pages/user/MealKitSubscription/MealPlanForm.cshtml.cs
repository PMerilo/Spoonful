using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Spoonful.Pages.user.MealKitSubscription
{
    public class MealPlanFormModel : PageModel
    {
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            return Page();
        }
    }
}
