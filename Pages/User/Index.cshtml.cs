using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Spoonful.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Spoonful.Pages.User
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        private readonly UserManager<CustomerUser> userManager;

        public IndexModel(UserManager<CustomerUser> userManager)
        {
            this.userManager = userManager;
        }
        public string UserName { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }


        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DOB { get; set; }



        public async Task<IActionResult> OnGetAsync(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user != null)
            {
                UserName = user.UserName;
                FirstName = user.FirstName;
                LastName = user.LastName;
                Email = user.NormalizedEmail.ToLower();
                Phone = user.PhoneNumber;
                DOB = user.DOB;
                return Page();
            } else
            {
                return RedirectToPage("/Error");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(UserName);
                if (UserName != null) user.UserName = UserName ;
                if (FirstName != null) user.FirstName = FirstName ;
                if (LastName != null) user.LastName = LastName ;
                if (Email != null) user.Email = Email ;
                if (Phone != null) user.PhoneNumber = Phone ;
                if (DOB != null) user.DOB = (DateTime)DOB;

                var result = await userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Failed to update details");
                    return Page();
                }
            }
            return RedirectToPage();
        }
    }
}
