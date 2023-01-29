using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Spoonful.Hubs;
using Spoonful.Models;

namespace Spoonful.Services
{
    public class NotificationService
    {
        private readonly UserManager<CustomerUser> userManager;
        private readonly AuthDbContext _context;
        private readonly IHubContext<NotificationHub> _notificationHubContext;

        public NotificationService(AuthDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _notificationHubContext = hubContext;
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

        public void SaveNotification(Notification notification, string username)
        {
            CustomerUser user = _context.Users.Include(u => u.Notifications).FirstOrDefault(x => x.UserName == username);
            if (user == null) return;
            user.Notifications.Add(notification);
            _context.SaveChanges();
        }

        public void SaveNotification(Notification notification, ICollection<string> usernames)
        {
            foreach (var username in usernames)
            {
                CustomerUser user = _context.Users.FirstOrDefault(x => x.UserName == username);
                if (user == null) return;
                user.Notifications.Add(notification);
                _context.SaveChanges();
            }
        }

        public async Task SendNotificationAsync(Notification notification, string username)
        {
            await _notificationHubContext.Clients.User(username).SendAsync("PushNotification", new
            {
                body = notification.Body,
                title = notification.Title,
                dateCreated = notification.DateCreated,
                url = notification.Url
            });
            SaveNotification(notification,username);
        }

        public async Task SendNotificationAllAsync(Notification notification)
        {
            await _notificationHubContext.Clients.All.SendAsync("PushNotification", new
            {
                body = notification.Body,
                title = notification.Title,
                dateCreated = notification.DateCreated,
                url = notification.Url
            });
        }


    }

    
}
