using Server.Application.Common.Dtos.Announcement;

namespace Server.Application.Common.Interfaces.Hubs;

public interface IAnnouncementClient
{
    Task GetNewAnnouncement(AnnouncementDto announcementDto);
}