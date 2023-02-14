using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Pages.Admin.DeliveryManagement
{
    [Authorize]
    public class DeliveryRouting : PageModel
    {

        private readonly MealKitService _mealKitService;
        private readonly OrderService _orderService;
        private readonly DeliveryService _deliveryService;

        public DeliveryRouting(AuthDbContext db, MealKitService mealKitService, OrderService orderService, DeliveryService deliveryService)
        {
            _mealKitService = mealKitService;
            _orderService = orderService;
            _deliveryService = deliveryService;
        }

        public void OnGet()
        {
            List<string> postalCodes = new List<string>();
            postalCodes.Add("763115");
            postalCodes.Add("368025");
            postalCodes.Add("628770");
            postalCodes.Add("310145");
            postalCodes.Add("520147");
            postalCodes.Add("238858");
            //Dictionary<string, List<string>> SortedRoutes = new();
            //foreach (string postalCode in postalCodes)
            //{
            //    foreach (string Dname in DistrictNames)
            //    {
            //        foreach (string dis in Districts[Dname])
            //        {
            //            if (!SortedRoutes.ContainsKey(Dname))
            //            {
            //                SortedRoutes[Dname] = new List<string>();
            //            }
            //            if (postalCode.Substring(0, 2) == dis)
            //            {
            //                SortedRoutes[Dname].Add(postalCode);
            //                Console.WriteLine(postalCode + " " + dis);
            //            }
            //        }
            //    }
            //}
            //Dictionary<string, List<string>> SortedRoutes = _deliveryService.GroupDeliveries(postalCodes);

            //foreach (string i in DistrictNames)
            //{
            //    foreach (string dis in SortedRoutes[i])
            //    {
            //        Console.WriteLine(i + " " + dis);
            //    }
            //}
        }

        public async Task<JsonResult> OnPostRetrieveRoutes(string route)
        {
            List<string> postalCodes = _deliveryService.GetOrderDetailsfortheDay();
            Dictionary<string, List<string>> SortedRoutes = _deliveryService.GroupDeliveries(postalCodes);
            List<string> Sroute = SortedRoutes[route];
            Sroute.Sort();
            Console.WriteLine(route);
            return new JsonResult(new { List = Sroute });
        }
    }
}
