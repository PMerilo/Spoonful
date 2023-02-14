using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spoonful.Models;
using System.ComponentModel.DataAnnotations;

namespace Spoonful.Pages.Account
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly UserManager<CustomerUser> userManager;
        private IWebHostEnvironment _environment;
        public IndexModel(UserManager<CustomerUser> userManager, IWebHostEnvironment environment)
        {
            this.userManager = userManager;
            _environment = environment;
        }
        [BindProperty]
        public string? UserName { get; set; }

        [BindProperty]
        public string? FirstName { get; set; }

        [BindProperty]
        public string? LastName { get; set; }


        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }

        [BindProperty]
        [DataType(DataType.DateTime)]
        public DateTime? DOB { get; set; }

        [BindProperty]
        public string? ImageURL { get; set; }

        [BindProperty]
        public IFormFile? Upload { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                UserName = user.UserName;
                FirstName = user.FirstName;
                LastName = user.LastName;
                Email = user.NormalizedEmail.ToLower();
                Phone = user.PhoneNumber;
                DOB = user.DOB;
                ImageURL = user.ImageURL ?? "/images/people/avatar-1.png";
                return Page();
            }
            else
            {
                return RedirectToPage("/Error");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TempData["FlashMessage.Text"] = "Changes Failed!";
                TempData["FlashMessage.Type"] = "danger";
                return RedirectToPage();
            }

            var user = await userManager.GetUserAsync(User);
            if (UserName != null) user.UserName = UserName;
            if (FirstName != null) user.FirstName = FirstName;
            if (LastName != null) user.LastName = LastName;
            if (DOB != null) user.DOB = (DateTime)DOB;

            if (Upload != null)
            {

                if (Upload.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("Upload",
                    "File size cannot exceed 2MB.");
                    return Page();
                }
                var uploadsFolder = "uploads";
                var imageFile = Guid.NewGuid() + Path.GetExtension(
                Upload.FileName);
                var imagePath = Path.Combine(_environment.ContentRootPath,
                "wwwroot", uploadsFolder, imageFile);
                using var fileStream = new FileStream(imagePath,
                FileMode.Create);
                await Upload.CopyToAsync(fileStream);
                if (user.ImageURL != null)
                {
                    var oldImageFile = Path.GetFileName(user.ImageURL);
                    var oldImagePath = Path.Combine(
                    _environment.ContentRootPath, "wwwroot", uploadsFolder, oldImageFile);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                user.ImageURL = string.Format("/{0}/{1}", uploadsFolder,
                imageFile);

            }

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Failed to update details");
                return Page();
            }
            TempData["FlashMessage.Text"] = "Changes saved!";
            TempData["FlashMessage.Type"] = "success";
            return RedirectToPage();
        }

        
    }
}
