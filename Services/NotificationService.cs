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

        public void SendNotification(Notification notification, String username)
        {
            CustomerUser user = _context.Users.FirstOrDefault(x => x.UserName == username);
            if (user == null) return;
            user.Notifications.Add(notification);
            _context.SaveChanges();
        }


    }

    
}
