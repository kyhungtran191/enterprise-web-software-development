using Microsoft.AspNetCore.SignalR;

namespace Server.Application.Common.Interfaces.Hubs.Announcement;

public class AnnouncementHub : Hub<IAnnouncementClient>
{
}