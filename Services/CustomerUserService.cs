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
        public static readonly List<string> Protected = new List<string>
        {
            Customer,
            Admin,
            Driver,
            RootUser
        }; 
    }

	public static class PasswordPolicy
	{
		public static readonly TimeSpan MaxAge = TimeSpan.FromDays(90);
	}

	public class CustomerUserService
    {
        private readonly SignInManager<CustomerUser> _signInManager;
        private readonly UserManager<CustomerUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AuthDbContext _db;
		private readonly IPasswordHasher<CustomerUser> _passwordHasher;

		public CustomerUserService(SignInManager<CustomerUser> signInManager, UserManager<CustomerUser> userManager, AuthDbContext db, RoleManager<IdentityRole> roleManager, IPasswordHasher<CustomerUser> passwordHasher)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _passwordHasher = passwordHasher;
        }

        public List<CustomerUser> GetAll()
        {
            return _db.Users.ToList();
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

		public void UpdateLastPassChanged(string UserName)
		{
			var user = _db.Users.FirstOrDefault(u => u.UserName == UserName);
			if (user == null)
			{
				return;
			}
			user.LastPassChanged = DateTimeOffset.UtcNow;
			_db.SaveChanges();
		}
        public async Task<bool> ValidateLastPassChangedAsync(string UserName)
		{
            var user = await _userManager.FindByNameAsync(UserName);
            return user.LastPassChanged.Add(PasswordPolicy.MaxAge).CompareTo(DateTimeOffset.UtcNow) < 0;
		}

		public bool ValidatePreviousPassword(string UserName, string NewPassword)
		{
			var user = _db.Users.Include(u => u.PreviousPassword).FirstOrDefault(x => x.UserName == UserName);
			if (user == null)
			{
				return false;
			}
			var passwordQ = user.PreviousPassword.OrderBy(x => x.DateCreated).ToList();
			if (passwordQ.Count == 2)
			{
				passwordQ.RemoveAt(0);
			}
			passwordQ.Add(new PreviousPassword
			{
				Password = user.PasswordHash,
			});
			foreach (var item in passwordQ)
			{
				var result = _passwordHasher.VerifyHashedPassword(user, item.Password, NewPassword);
				if (result == PasswordVerificationResult.Success)
				{
					return false;
				};
			}
			return true;
		}

		public void UpdatePreviousPassword(string UserName)
		{
			var user = _db.Users.Include(u => u.PreviousPassword).FirstOrDefault(x => x.UserName == UserName);
			if (user == null)
			{
				return;
			}
			var passwordQ = user.PreviousPassword.OrderBy(x => x.DateCreated).ToList();
			if (passwordQ.Count == 2)
			{
				passwordQ.RemoveAt(0);
			}
			passwordQ.Add(new PreviousPassword
			{
				Password = user.PasswordHash,
			});
			user.PreviousPassword = passwordQ.ToList();
			UpdateLastPassChanged(UserName);
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
