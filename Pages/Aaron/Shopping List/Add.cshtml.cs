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
        private readonly ShoppingListService _shoppingListService;

        public AddModel(ShoppingListService shoppingListService)
        {
            _shoppingListService = shoppingListService;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {

                _shoppingListService.AddEntry(ShoppingEntry);
                TempData["FlashMessage.Type"] = "success";
                return Redirect("/Aaron/Shopping%20List/ShoppingList");
            }
            return Page();
        }
    }
}

