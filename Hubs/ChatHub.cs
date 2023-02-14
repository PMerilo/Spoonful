using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Hubs
{
    [AllowAnonymous]
    public class ChatHub : Hub
    {
        private readonly AuthDbContext _context;
        private readonly MessagingService messagingService;

        public ChatHub(AuthDbContext context, MessagingService messagingService)
        {
            _context = context;
            this.messagingService = messagingService;
        }

        public override Task OnConnectedAsync()
        {
            //Console.WriteLine("Connected: "+Context.UserIdentifier);
            return base.OnConnectedAsync();
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
                        message =item.Message
                    });
                }
                await Clients.Caller.SendAsync("RetrieveChat", list);
            }
            
        }
        public async Task SendMessage(string username, string message, string sender)
        {
            await messagingService.CreateMessage(sender, username, message);
            //Console.WriteLine("Sending to: " + username);
            await Clients.User(username).SendAsync("ReceiveMessage", username, message);
        }

        public async Task ReadMessage(int id)
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
