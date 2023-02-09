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
        public IActionResult OnGet()
        {
            ShoppingList = _shoppingListService.GetAllByCat();
            foreach (var entry in ShoppingList)
            {
                _shoppingListService.DeleteEntry(entry);

            }

            return Redirect("/Aaron/Shopping%20List/ShoppingList");
        }
    }
}
