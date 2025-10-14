using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace BlogMagangementSystem.Features.SignalrFeature;

[HubName("Chat")]
public  class ChatHub : Hub
{
    private const string UserId = "Noha";

    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync(user, message);
        await Clients.Caller.SendAsync( user, message);
        await Clients.Others.SendAsync(user, message);
        await Clients.Group("Group01").SendAsync(user, message);
        await Clients.User(UserId).SendAsync(user, message);

    }

    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).SendAsync($"{Context.ConnectionId} has joined the group {groupName}.");

    }
}
