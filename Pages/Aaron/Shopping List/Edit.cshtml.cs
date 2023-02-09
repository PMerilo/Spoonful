using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.Aaron.Shopping_List
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public ShoppingEntry Shopping { get; set; }

        private readonly ShoppingListService _shoppingListService;

        public EditModel(ShoppingListService shoppingListService)
        {
            _shoppingListService = shoppingListService;
        }
        public IActionResult OnGet(int entryId)
        {
            ShoppingEntry? entry = _shoppingListService.GetEntryById(entryId);
            if (entry != null)
            {
                Shopping = entry;
                return Page();
            }
            else
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = string.Format("Entry ID {0} not found", entryId);
                return Redirect("/Aaron/Shopping%20List/ShoppingList");
            }
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {

                _shoppingListService.UpdateEntry(Shopping);
                TempData["FlashMessage.Type"] = "success";
                return Redirect("/Aaron/Shopping%20List/ShoppingList");
            }
            return Page();
        }
    }
}

