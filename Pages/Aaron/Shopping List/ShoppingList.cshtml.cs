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
        public int ListId { get; set; }

        private readonly ShoppingListService _shoppingService;

        public ShoppingListModel(ShoppingListService shoppingListService)
        {
            _shoppingService = shoppingListService;
        }

        public List<ShoppingEntry> ShoppingList { get; set; } = new();

        public void OnGet()
        {
            ShoppingList = _shoppingService.GetAllByCat();
        }

        public IActionResult OnPost()
        {
            switch (sortFilter)
            {
                case "name":
                    ShoppingList = _shoppingService.GetAllByName();
                    break;

                case "category":
                    ShoppingList = _shoppingService.GetAllByCat();
                    break;

                case "purchase":
                    ShoppingList = _shoppingService.GetAllByPurchase();
                    break;

                default:
                    ShoppingList = _shoppingService.GetAll();
                    break;
            }

            var ShoppingBoughtEntry = _shoppingService.GetEntryById(ListId);
            ShoppingBoughtEntry.isBought = !ShoppingBoughtEntry.isBought;
            _shoppingService.UpdateEntry(ShoppingBoughtEntry);


            return Page();
        }
    }
}