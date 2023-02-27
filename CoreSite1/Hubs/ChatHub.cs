using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreSite1.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SignalRChat.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        ////https://docs.microsoft.com/en-us/aspnet/signalr/overview/guide-to-the-api/mapping-users-to-connections
        ///
        public readonly static ConnectionMapping<string> _connections =
        new ConnectionMapping<string>();

        public Task SendMessageToGroup(string groupName, string message)
        {
            return Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId}: {message}");
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} and {Context.User.Identity.Name/*Context.User.GetLoggedInUserId<string>()*/} has joined the group {groupName}.");
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} and {Context.User.Identity.Name}  has left the group {groupName}.");
        }

        public Task SendPrivateMessage(string user, string message)
        {
            
            return Clients.User(user).SendAsync("ReceiveMessage", message);
        }

        public Task SendMessageToUser(string user, string message)
        {
            //string name = Context.User.Identity.Name;

            foreach (var connectionId in _connections.GetConnections(user))
            {
                Clients.Client(connectionId).SendAsync("ReceiveMessage", message,Context.User.Identity.Name);
            }
            return Clients.Client(user).SendAsync("ReceiveMessage", message);
            //return Clients.Caller.SendAsync("ReceiveMessage", user, message);
            

            
        }

        #region OnConnectedAsync
        public override async Task OnConnectedAsync()
        {
                string name = Context.User.Identity.Name;
                _connections.Add(name, Context.ConnectionId);//Context.User.GetLoggedInUserId<string>()
                await base.OnConnectedAsync();
        }
        #endregion

        #region OnDisconnectedAsync
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string name = Context.User.Identity.Name; //Context.User.GetLoggedInUserId<string>()
            _connections.Remove(name, Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
        #endregion

        /////Below Method added For Broadcast
        ///
        public Task SendMessage(string user, string message)
        {
            return Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public Task SendMessageToCaller(string message)
        {
            return Clients.Caller.SendAsync("ReceiveMessage", message);
        }
    }
}
