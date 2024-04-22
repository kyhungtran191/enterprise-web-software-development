using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Server.Application.Common.Interfaces.Hubs.Announcement;

[Authorize]
public class AnnouncementHub : Hub<IAnnouncementClient>
{
}