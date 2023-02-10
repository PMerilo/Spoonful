using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.Aaron.Shopping_List
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public ShoppingEntry ShoppingEntry { get; set; }

        private readonly ShoppingListService _shoppingListService;

        public DeleteModel(ShoppingListService shoppingListService)
        {
            _shoppingListService = shoppingListService;
        }
        public IActionResult OnGet(int id)
        {
            ShoppingEntry? entry = _shoppingListService.GetEntryById(id);
            string userIdvar = entry.userId;
            if (entry != null)
            {
                ShoppingEntry = entry;
                _shoppingListService.DeleteEntry(entry);
                string userUrl = "/User/Shopping%20List/Index?id=" + userIdvar;
                return Redirect(userUrl);
            }
            else
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = string.Format("Entry ID {0} not found", id);
                string userUrl = "/User/Shopping%20List/Index?id=" + userIdvar;
                return Redirect(userUrl);
            }
        }
    }
}
