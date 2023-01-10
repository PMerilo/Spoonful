using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Spoonful.Pages.user.MealKitSubscription
{

    [Authorize]
    public class OrderModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
