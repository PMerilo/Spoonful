using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.Aaron.Shopping_List
{
    [AllowAnonymous]
    public class AddModel : PageModel
    {
        [BindProperty]
        public ShoppingEntry ShoppingEntry { get; set; }
        [BindProperty]
        public string userIdvar { get; set; }
        private readonly ShoppingListService _shoppingListService;

        public AddModel(ShoppingListService shoppingListService)
        {
            _shoppingListService = shoppingListService;
        }
        public void OnGet(string userId)
        {
            ShoppingEntry.userId = userId;
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {

                _shoppingListService.AddEntry(ShoppingEntry);
                TempData["FlashMessage.Type"] = "success";
                string userUrl = "/User/Shopping%20List/Index?id=" + userIdvar;
                return Redirect(userUrl);
            }
            return Page();
        }
    }
}

