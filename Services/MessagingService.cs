using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using Spoonful.Hubs;
using Spoonful.Models;

namespace Spoonful.Services
{
    public class MessagingService
    {
        private readonly UserManager<CustomerUser> userManager;
        private readonly AuthDbContext _context;


        public MessagingService(AuthDbContext context, UserManager<CustomerUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        public List<Messages> GetChat(string sender, string receiver)
        {
            var msgs = _context.Messages.Include(m => m.Sender).Include(m => m.Receiver).Where(u => (u.Sender.UserName == sender && u.Receiver.UserName == receiver) || (u.Sender.UserName == receiver && u.Receiver.UserName == sender)).ToList();
            return msgs;
        }

        public async Task<List<CustomerUser>> GetChats(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            var msgs = _context.Messages.Include(m => m.Sender).Include(m => m.Receiver).Where(u => u.Sender.UserName == username).Select(u => u.Receiver).Distinct();
            var msgs2 = _context.Messages.Include(m => m.Sender).Include(m => m.Receiver).Where(u => u.Receiver.UserName == username).Select(u => u.Sender).Distinct();
            var unionResult = msgs.Union(msgs2).ToList();
            return unionResult;
        }


        public async Task CreateMessage(string sender, string receiver, string message)
        {
            var senderUser = await userManager.FindByNameAsync(sender);
            var receiverUser = await userManager.FindByNameAsync(receiver);
            var msg = new Messages
            {
                Sender = senderUser,
                Receiver = receiverUser,
                Message = message
            };
            _context.Messages.Add(msg);
            _context.SaveChanges();
        }
    }
}
