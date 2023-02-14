using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Spoonful.Models;
using Spoonful.Services;
using System.Security.Policy;

namespace Spoonful.Hubs
{
	[AllowAnonymous]
	public class NotificationHub : Hub
	{
        private readonly AuthDbContext _context;
        private readonly MessagingService messagingService;
        private readonly NotificationService notificationService;
        private readonly UserManager<CustomerUser> userManager;

        public NotificationHub(AuthDbContext context, MessagingService messagingService, NotificationService notificationService, UserManager<CustomerUser> userManager)
        {
            _context = context;
            this.messagingService = messagingService;
            this.notificationService = notificationService;
            this.userManager = userManager;
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

        public async Task GetChat(string sender, string receiver)
        {
            var msgs = messagingService.GetChat(sender, receiver);
            var list = new List<object>();
            if (msgs != null)
            {
                foreach (var item in msgs)
                {
                    list.Add(new
                    {
                        sender = item.Sender.UserName,
                        receiver = item.Receiver.UserName,
                        message = item.Message
                    });
                }
                await Clients.Caller.SendAsync("RetrieveChat", list);
            }

        }
        public async Task SendMessage(string username, string message, string sender)
        {
            var senderU = await userManager.FindByNameAsync(sender);
            var receiver = await userManager.FindByNameAsync(username);
            await messagingService.CreateMessage(sender, username, message);
            var notif = new Notification
            {
                Title = $"New Message From @{sender}",
                Body = message,
                User = await userManager.FindByNameAsync(username),
                Url = $"/User/Chat/{senderU.Id}"
            };
            notificationService.SendNotificationAsync(notif, username, true);
            //Console.WriteLine("Sending to: " + username);
            await Clients.User(username).SendAsync("ReceiveMessage", username, message);
        }
    }
}
