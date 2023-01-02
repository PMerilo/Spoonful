using Microsoft.AspNetCore.Identity;
using Spoonful.Models;

namespace Spoonful.Services
{
    public class CustomerUserService
    {
        private readonly SignInManager<CustomerUser> signInManager;
        private readonly UserManager<CustomerUser> userManager;

        public CustomerUserService(SignInManager<CustomerUser> signInManager, UserManager<CustomerUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;

        }

        public List<CustomerUser> GetAll()
        {
            return userManager.Users.ToList();
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
