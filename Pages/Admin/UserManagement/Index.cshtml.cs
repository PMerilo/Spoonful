using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Spoonful.Models;
using Spoonful.Services;
using System.Security.Claims;

namespace Spoonful.Pages.Admin.UserManagement
{
    public class IndexModel : PageModel
    {
        private readonly AuthDbContext _db;
        private readonly UserManager<CustomerUser> _userManager;

        public IndexModel(AuthDbContext db, UserManager<CustomerUser> userManager)
        {
            _db = db;
            _userManager = userManager;

        }

        public List<CustomerUser> Users { get; set; }
        public List<CustomerDetails> CustomerDetails { get; set; }
        public List<AdminDetails> AdminDetails { get; set; }
        public List<DriverDetails> DriverDetails { get; set; }



        public async Task OnGet()
        {
            Users = _userManager.Users.Include(u => u.UserDetails).ToList();
            CustomerDetails = _db.CustomerDetails.Include(d => d.User).ToList();
            AdminDetails = _db.AdminDetails.Include(d => d.User).ToList();
            DriverDetails = _db.DriverDetails.Include(d => d.User).ToList();
            //foreach (var user in Users)
            //{
            //    var claim = await _userManager.GetClaimsAsync(user);
            //    Claims.Add(claim);
            //}
        }
    }
}
