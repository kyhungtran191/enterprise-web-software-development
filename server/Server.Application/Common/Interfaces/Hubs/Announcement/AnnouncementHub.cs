using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Server.Application.Common.Interfaces.Hubs.Announcement;

[Authorize]
public class AnnouncementHub : Hub
{
    public async Task SendMessage(string message) {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }
}