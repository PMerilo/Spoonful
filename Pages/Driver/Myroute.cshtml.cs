using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Spoonful.Models;
using Spoonful.Services;
using System.Text.Encodings.Web;

namespace Spoonful.Pages.Driver
{
    public class MyrouteModel : PageModel
    {
        private readonly AuthDbContext _db;
        private readonly DeliveryService _deliveryService;
        private readonly UserManager<CustomerUser> _userManager;
        private IWebHostEnvironment _environment;
        private readonly IEmailService emailService;

        public MyrouteModel(DeliveryService deliveryService, UserManager<CustomerUser> userManager, AuthDbContext db, IWebHostEnvironment environment, IEmailService emailService)
        {
            _deliveryService = deliveryService;
            _userManager = userManager;
            _db = db;
            _environment = environment;
            this.emailService = emailService;
        }

        public Routes MyRoute { get; set; }

        public List<Delivery> deliveries { get; set; } = new();

        public DriverDetails driverDetails { get; set; }

        public int? routeId { get; set; }

        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        public IFormFile? Upload { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            driverDetails = _db.DriverDetails.Include(u => u.User).FirstOrDefault(u => u.User.UserName == user.UserName);
            routeId = driverDetails.RoutesId;
            if(routeId != null)
            {
                MyRoute = _deliveryService.GetRouteById((int)routeId);
                deliveries = _deliveryService.GetAllDeliveriesForDriver((int)routeId);
                var count = 0;
                foreach (Delivery d in deliveries)
                {
                    if (d.status == "Delivered")
                    {
                        count++;
                    }
                }
                Console.WriteLine(count);
                if(count == deliveries.Count)
                {
                    MyRoute.Status = "Completed";
                    _deliveryService.UpdateRoute(MyRoute);
                    Console.WriteLine("Deliveries completed");
                }
            }else
            {
                routeId = -1;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                Delivery? delivery = _deliveryService.GetDeliveryById(Id);
                var user = await _userManager.FindByIdAsync(delivery.OrderDetails.userId);
                Console.WriteLine(user.Email);
                if (Upload != null)
                {
                    if (Upload.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("Upload",
                        "File size cannot exceed 2MB.");
                        return Page();
                    }
                    var uploadsFolder = "uploads/deliveryUpload";
                    var imageFile = Guid.NewGuid() + Path.GetExtension(
                    Upload.FileName);
                    var imagePath = Path.Combine(_environment.ContentRootPath,
                    "wwwroot", uploadsFolder, imageFile);
                    using var fileStream = new FileStream(imagePath, FileMode.Create);
                    await Upload.CopyToAsync(fileStream);
                    delivery.ConfirmationImageURL = string.Format("/{0}/{1}", uploadsFolder, imageFile);
                }
                delivery.deliveryDateTime = DateTime.Now.ToString();
                delivery.deliveryConfirmation = "true";
                delivery.status = "Delivered";
                var code = delivery.Id;
                var callbackUrl = Url.Page(
                "/User/MealKitSubscription/DeliveryConfirmation",
                pageHandler: null,
                values: new { username = user.UserName, code = code },
                protocol: Request.Scheme);
                var resultEmail = emailService.SendEmail(
                            user.Email,
                            "Delivery Confirmation",
                            $"Please confirm that you have gotten your package by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.",
                            null,
                            null);


                _deliveryService.UpdateDelivery(delivery);
                TempData["FlashMessage.Type"] = "success";
                /*TempData["FlashMessage.Text"] = string.Format("Vouchers {0} is added", MyEmployee.Name);*/
                return Redirect("/Driver/Myroute");
            }
            TempData["FlashMessage.Type"] = "error";
            TempData["FlashMessage.Text"] = "Modal failed";
            return Page();
        }

        public async Task<JsonResult> OnPostRetrieveRoute(string route)
        {
            Console.WriteLine(route);
            if(route != "1")
            {
                List<string> postalCodes = _deliveryService.GetOrderDetailsfortheDay();
                Dictionary<string, List<string>> SortedRoutes = _deliveryService.GroupDeliveries(postalCodes);
                List<string> Sroute = SortedRoutes[route];
                Sroute.Sort();
                Console.WriteLine(route);
                return new JsonResult(new { List = Sroute });
            }
            else
            {
                return new JsonResult(new { List = "null" });
            }
        }
    }
}
