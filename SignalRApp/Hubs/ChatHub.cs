using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRApp.Data;

namespace SignalRApp.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;
        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task SendMessageToAll(string user , string message)
        {
            await Clients.All.SendAsync("MessageRecieve" , user,message);
        }
        [Authorize]
        public async Task SendMessageToReciever(string sender ,string reciever , string message)
        {
            var userId = _context.Users.FirstOrDefault(u => u.Email.ToLower() == reciever.ToLower()).Id;

            if(!string.IsNullOrEmpty(userId) )
            {
                await Clients.User(userId).SendAsync("MessageRecieve",sender, message);
            }
            
        }
    }
}
