using Server.Application.Common.Dtos.Announcement;
using Server.Application.Wrappers.PagedResult;

namespace Server.Application.Common.Interfaces.Services;

public interface IAnnouncementService
{
    Task<PagedResult<AnnouncementDto>> GetAllUnreadPaging(Guid userId, int pageIndex, int pageSize);

    Task<bool> MarkAsRead(Guid userId, string id);

    void Add(AnnouncementDto announcementDto);    
    void AddToAnnouncementUsers(IEnumerable<AnnouncementUserDto> announcementUserDtos);
}