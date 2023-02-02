using Microsoft.AspNetCore.SignalR;

namespace Spoonful.Services
{
    public class MyUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User.Identity.Name;
        }
    }
}
