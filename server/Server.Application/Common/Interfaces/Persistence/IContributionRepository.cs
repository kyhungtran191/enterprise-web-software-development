using Server.Application.Common.Dtos.Contributions;
using Server.Application.Common.Dtos.Tags;
using Server.Application.Wrappers.PagedResult;
using Server.Domain.Entity.Content;

namespace Server.Application.Common.Interfaces.Persistence
{
    public interface IContributionRepository : IRepository<Contribution,Guid>
    {
        Task<PagedResult<ContributionInListDto>> GetAllPaging(string? keyword, Guid? yearId, int pageIndex = 1, int pageSize = 10);
        
        Task<ContributionDto> GetContributionByTitle(string title);

        Task<List<TagDto>> GetAllTags(Guid contributionId);
    }
}
