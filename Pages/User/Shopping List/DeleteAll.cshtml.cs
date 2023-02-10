using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Migrations;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.Aaron.Shopping_List
{
    public class DeleteAllModel : PageModel
    {
        [BindProperty]
        public ShoppingEntry ShoppingEntry { get; set; }

        private readonly ShoppingListService _shoppingListService;

        public List<ShoppingEntry> ShoppingList { get; set; } = new();

        public DeleteAllModel(ShoppingListService shoppingListService)
        {
            _shoppingListService = shoppingListService;
        }
        public IActionResult OnGet(string userId)
        {
            ShoppingList = _shoppingListService.GetAllByCat(userId);

            string userIdvar = "";
            foreach (var entry in ShoppingList)
            {
                userIdvar = entry.userId;
                _shoppingListService.DeleteEntry(entry);

            }

            string userUrl = "/User/Shopping%20List/Index?id=" + userIdvar;
            return Redirect(userUrl);
        }
    }
}
