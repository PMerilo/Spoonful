using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Spoonful.Models;
using Spoonful.Services;
using System.Text;

namespace Spoonful.Pages.User.MealKitSubscription
{
    [AllowAnonymous]
    public class DeliveryConfirmationModel : PageModel
    {
        private readonly SignInManager<CustomerUser> signInManager;
        private readonly UserManager<CustomerUser> userManager;
        private readonly DeliveryService _deliveryService;
        public DeliveryConfirmationModel(SignInManager<CustomerUser> signInManager, UserManager<CustomerUser> userManager, DeliveryService deliveryService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            _deliveryService = deliveryService;

        }
        public async Task OnGetAsync(string username, int code)
        {
            var user = await userManager.FindByNameAsync(username);

            if (user == null)
            {
                TempData["FlashMessage.Text"] = "Invalid Tokens";
                TempData["FlashMessage.Type"] = "danger";
            }

            var deliveryId = code;
            var delivery = _deliveryService.GetDeliveryById(deliveryId);
            delivery.customerConfirmation = "true";
            _deliveryService.UpdateDelivery(delivery);

            Redirect("/");
        }
    }
}
