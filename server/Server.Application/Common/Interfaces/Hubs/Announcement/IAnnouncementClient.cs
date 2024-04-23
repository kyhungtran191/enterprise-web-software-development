using Server.Application.Common.Dtos.Announcement;

namespace Server.Application.Common.Interfaces.Hubs.Announcement;

public interface IAnnouncementClient
{
    Task GetNewAnnouncement(AnnouncementDto announcementDto);

    Task SendMessage(string message);
}