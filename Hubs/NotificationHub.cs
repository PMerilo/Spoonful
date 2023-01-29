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
            Console.WriteLine(Context.UserIdentifier);
            return base.OnConnectedAsync();
        }

        public async Task GetNotifications()
        {
            var notifications = _context.Notifications.Where(n => n.User.UserName == Context.UserIdentifier).ToList();
            if (notifications != null && notifications.Count > 0)
            {
                await Clients.Caller.SendAsync("RetrieveNotifications", notifications );
            }
        }
    }
}
