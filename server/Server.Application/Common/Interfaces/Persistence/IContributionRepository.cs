using Server.Application.Common.Dtos.Contributions;
using Server.Application.Common.Dtos.Tags;
using Server.Application.Wrappers.PagedResult;
using Server.Domain.Entity.Content;

namespace Server.Application.Common.Interfaces.Persistence
{
    public interface IContributionRepository : IRepository<Contribution,Guid>
    {
        Task<PagedResult<ContributionInListDto>> GetAllPaging(string? keyword, Guid? yearId, int pageIndex = 1, int pageSize = 10);
        Task<ContributionDto> GetContributionBySlug(string slug);

        Task<List<TagDto>> GetAllTags(Guid contributionId);
        Task<List<ContributionDto>> GetPopularContributionAsync(int count);
        Task<bool> IsSlugAlreadyExisted(string slug, Guid? id = null);
        Task Approve(Guid contributionId,Guid userId);
        Task SendToApprove(Guid contributionId,Guid userId);
        Task Reject(Guid contributionId,Guid userId, string note);
        Task<string> GetRejectReason(Guid contributionId);
    }
}
