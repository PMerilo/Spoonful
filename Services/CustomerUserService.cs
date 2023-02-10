using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Spoonful.Models;

namespace Spoonful.Services
{
    public static class Roles
    {
        public const string Customer = "Customer";
        public const string Admin = "Admin";
        public const string Driver = "Driver";
        public const string RootUser = "RootUser";

    }

    public class CustomerUserService
    {
        private readonly SignInManager<CustomerUser> _signInManager;
        private readonly UserManager<CustomerUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AuthDbContext _db;

        

        public CustomerUserService(SignInManager<CustomerUser> signInManager, UserManager<CustomerUser> userManager, AuthDbContext db, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        public List<CustomerUser> GetAll()
        {
            return _userManager.Users.ToList();
        }

        public async Task SetUserRoleAsync(string UserName, string Role)
        {
            var user = _db.Users.Include(u => u.UserDetails).FirstOrDefault(x => x.UserName == UserName);
            if (user == null || user.UserDetails != null)
            {
                return;
            }
            if (!await _roleManager.RoleExistsAsync(Roles.Customer))
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.Customer));

            }
            if (!await _roleManager.RoleExistsAsync(Roles.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.Admin));

            }
            if (!await _roleManager.RoleExistsAsync(Roles.Driver))
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.Driver));

            }
            if (!await _roleManager.RoleExistsAsync(Roles.RootUser))
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.RootUser));

            }
            switch (Role)
            {
                case Roles.Customer:
                    user.UserDetails = new CustomerDetails()
                    {
                        UserId = user.Id
                    };
                    await _userManager.AddToRoleAsync(user, Roles.Customer);
                    break;
                case Roles.Admin:
                    user.UserDetails = new AdminDetails()
                    {
                        UserId = user.Id
                    };
                    await _userManager.AddToRoleAsync(user, Roles.Admin);
                    break;
                case Roles.Driver:
                    user.UserDetails = new DriverDetails()
                    {
                        UserId = user.Id
                    };
                    await _userManager.AddToRoleAsync(user, Roles.Driver);
                    break;
                case Roles.RootUser:
                    user.UserDetails = new AdminDetails()
                    {
                        UserId = user.Id
                    };
                    await _userManager.AddToRoleAsync(user, Roles.RootUser);
                    await _userManager.AddToRoleAsync(user, Roles.Admin);
                    break;
                default:
                    break;
            }
            var i = _db.SaveChanges();
            Console.WriteLine(i);
        }

        public void UpdateLastLogin(string UserName)
        {
            var user = _db.Users.FirstOrDefault(u=> u.UserName == UserName);
            if (user == null)
            {
                return;
            }
            user.LastLogin = DateTimeOffset.UtcNow;
            _db.SaveChanges();
        }


        public void Add(CustomerUser user)
        {

        }

        public void Update(CustomerUser user)
        {

        }

        public void Delete(int id)
        {

        }
    }

    
}
