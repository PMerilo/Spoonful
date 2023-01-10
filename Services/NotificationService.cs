using Microsoft.AspNetCore.Identity;
using Spoonful.Models;

namespace Spoonful.Services
{
    public class NotificationService
    {
        private readonly UserManager<CustomerUser> userManager;
        private readonly AuthDbContext _context;

        public NotificationService(AuthDbContext context)
        {
            _context = context;
        }

        public List<Notification> GetAll()
        {
            return _context.Notifications.OrderBy(n => n.DateCreated).ToList();
        }

        public List<Notification> GetUserNotifications(string name)
        {
            return _context.Notifications.Where(n => n.User.UserName == name).OrderBy(n => n.DateCreated).ToList();
        }

        public void SendNotification(string title, string body, DateTime datecreated, CustomerUser user, )
        {
            _context.Category.Add(category);
            _context.SaveChanges();
        }

        public void UpdateCategory(Category category)
        {
            _context.Category.Update(category);
            _context.SaveChanges();
        }


    }

    
}
