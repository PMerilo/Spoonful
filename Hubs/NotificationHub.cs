using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Spoonful.Models;
using System.Security.Policy;

namespace Spoonful.Hubs
{
	[AllowAnonymous]
	public class NotificationHub : Hub
	{
        private readonly AuthDbContext _context;

        public NotificationHub(AuthDbContext context)
        {
            _context = context;
        }

        public override Task OnConnectedAsync()
        {
            //Console.WriteLine(Context.UserIdentifier);
            return base.OnConnectedAsync();
        }

        public async Task GetNotifications()
        {
            var notifications = _context.Notifications.Where(n => n.User.UserName == Context.UserIdentifier).OrderByDescending(n => n.DateCreated).ToList();
            if (notifications != null && notifications.Count > 0)
            {
                await Clients.Caller.SendAsync("RetrieveNotifications", notifications );
            }
        }

        public async Task ReadNotification(int id)
        {
            var notification = _context.Notifications.Where(n => n.Id == id).FirstOrDefault();
            Console.WriteLine(notification.Seen);
            if (notification != null)
            {
                notification.Seen = true;
                _context.Notifications.Update(notification);
                _context.SaveChanges();
            }
        }
    }
}
