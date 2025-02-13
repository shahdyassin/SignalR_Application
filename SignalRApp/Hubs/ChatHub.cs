using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using NuGet.Protocol.Plugins;
using SignalRApp.Data;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SignalRApp.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;
        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }
        public override Task OnConnectedAsync()
        {
            var UserId  = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(!string.IsNullOrEmpty(UserId))
            {
                var userName = _context.Users.FirstOrDefault(x => x.Id == UserId).UserName;
                Clients.Users(HubConnections.OnlineUsers()).SendAsync("ReceiveUserConnected", UserId, userName);

                HubConnections.AddUserConnection(UserId, Context.ConnectionId);
            }
            return base.OnConnectedAsync();
        } 
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var UserId  = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);


            if(HubConnections.HasUserConnection(UserId,Context.ConnectionId))
            {
                var UserConnections = HubConnections.Users[UserId];

                UserConnections.Remove(Context.ConnectionId);


                HubConnections.Users.Remove(UserId);


                if (UserConnections.Any())
                {
                    HubConnections.Users.Add(UserId, UserConnections);
                }
            }

            if(!string.IsNullOrEmpty(UserId))
            {
                var userName = _context.Users.FirstOrDefault(x => x.Id == UserId).UserName;
                Clients.Users(HubConnections.OnlineUsers()).SendAsync("ReceiveUserDisConnected", UserId, userName);

                HubConnections.AddUserConnection(UserId, Context.ConnectionId);
            }
            return base.OnDisconnectedAsync(exception);
        }
        public async Task SendAddRoomMessage(int maxRoom,int roomId, string roomName)
        {
            var UserId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = _context.Users.FirstOrDefault(x => x.Id == UserId).UserName;


            await Clients.All.SendAsync("ReceiveAddRoomMessage",maxRoom, roomId, roomName, UserId,userName);
        } 
        public async Task SendDeleteRoomMessage(int deleted,int selected, string roomName)
        {
            var UserId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = _context.Users.FirstOrDefault(x => x.Id == UserId).UserName;


            await Clients.All.SendAsync("ReceiveDeleteRoomMessage",deleted, selected, roomName,userName);
        }

        public async Task SendPublicMessage(int roomId, string message, string roomName)
        {
            var UserId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = _context.Users.FirstOrDefault(x => x.Id == UserId).UserName;

            await Clients.All.SendAsync("ReceivePublicMessage", roomId, UserId, userName, message, roomName);

        }
        public async Task SendPrivateMessage(string receiverId, string message, string receiverName)
        {
            var senderId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var senderName = _context.Users.FirstOrDefault(u => u.Id == senderId).UserName;

            var users = new string[] { senderId, receiverId };

            await Clients.Users(users).SendAsync("ReceivePrivateMessage", senderId, senderName, receiverId, message, Guid.NewGuid(), receiverName);
        }
        public async Task SendOpenPrivateChat(string receiverId)
        {
            var username = Context.User.FindFirstValue(ClaimTypes.Name);
            var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await Clients.User(receiverId).SendAsync("ReceiveOpenPrivateChat", userId, username);
        }

        public async Task SendDeletePrivateChat(string chartId)
        {
            await Clients.All.SendAsync("ReceiveDeletePrivateChat", chartId);
        }




        //public async Task SendMessageToAll(string user , string message)
        //{
        //    await Clients.All.SendAsync("MessageRecieve" , user,message);
        //}
        //[Authorize]
        //public async Task SendMessageToReciever(string sender ,string reciever , string message)
        //{
        //    var userId = _context.Users.FirstOrDefault(u => u.Email.ToLower() == reciever.ToLower()).Id;

        //    if(!string.IsNullOrEmpty(userId) )
        //    {
        //        await Clients.User(userId).SendAsync("MessageRecieve",sender, message);
        //    }

        //}
    }
}
