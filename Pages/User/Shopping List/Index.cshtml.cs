using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.Aaron.Shopping_List
{
    [AllowAnonymous]
    public class ShoppingListModel : PageModel
    {
        [BindProperty]
        public ShoppingEntry ShoppingEntry { get; set; }

        [BindProperty]
        public string sortFilter { get; set; }

        [BindProperty]
        public bool boughtCheckBox { get; set; }

        [BindProperty]
        public string entryUserId { get; set; }
        public string userIdvar { get; set; }

        [BindProperty]
        public int ListId { get; set; }

        private readonly ShoppingListService _shoppingService;

        public ShoppingListModel(ShoppingListService shoppingListService)
        {
            _shoppingService = shoppingListService;
        }

        public List<ShoppingEntry> ShoppingList { get; set; } = new();

        public void OnGet(string id)
        {
            userIdvar = id;
            ShoppingList = _shoppingService.GetAllByCat(userIdvar);
        }

        public IActionResult OnPost()
        {

            userIdvar = entryUserId;
            var ShoppingBoughtEntry = _shoppingService.GetEntryById(ListId);
            ShoppingBoughtEntry.isBought = !ShoppingBoughtEntry.isBought;
            _shoppingService.UpdateEntry(ShoppingBoughtEntry);
            ShoppingList = _shoppingService.GetAllByCat(userIdvar);

            return Page();
        }
    }
}