using Server.Application.Common.Dtos.Tags;
using Server.Application.Wrappers.PagedResult;
using Server.Domain.Entity.Content;

namespace Server.Application.Common.Interfaces.Persistence
{
    public interface ITagRepository : IRepository<Tag,Guid>
    {
        Task<Tag> GetTagByName(string name);
        Task<PagedResult<TagDto>> GetAllTagsPaging(string? keyword, int pageIndex = 1, int pageSize = 10);
    }
}
