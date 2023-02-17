using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;
using System.Text.RegularExpressions;

namespace Spoonful.Pages.Admin.DeliveryManagement
{
    public class IndexModel : PageModel
    {
        private readonly MealKitService _mealKitService;
        private readonly OrderService _orderService;
        private readonly DeliveryService _deliveryService;

        public IndexModel(AuthDbContext db, MealKitService mealKitService, OrderService orderService, DeliveryService deliveryService)
        {
            _mealKitService = mealKitService;
            _orderService = orderService;
            _deliveryService = deliveryService;
        }

        public List<Routes> routeList { get; set; } = new();

        public List<Delivery> deliveryList { get; set; } = new();
        public void OnGet()
        {
            routeList = _deliveryService.GetAllRoutes();
            deliveryList = _deliveryService.GetAllDeliveriesWithIncludes();
        }

        public async Task<JsonResult> OnPostCreateDeliveryRoutes(string route)
        {
            //find postal code in address
            string address = "115C Yishun Ring Road, #03-803, Singapore 763115";
            string pattern = @"\b\d{6}\b";
            Match match = Regex.Match(address, pattern);
            if (match.Success)
            {
                Console.WriteLine("Postal code: " + match.Value);
            }

            //OrderDetails deets = _deliveryService.GetOrderDetailsbyPostalCode("763115");
            //Console.WriteLine(deets.Address);

            List<string> postalCodes = _deliveryService.GetOrderDetailsfortheDay();
            _deliveryService.CreateRoutes(postalCodes);
            //List<string> Sroute = new();
            Console.WriteLine(route);
            return new JsonResult(new { status = "Completed" });
        }
    }
}
