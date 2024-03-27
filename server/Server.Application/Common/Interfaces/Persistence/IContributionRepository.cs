using Server.Application.Common.Dtos.Contributions;
using Server.Application.Common.Dtos.Tags;
using Server.Application.Wrappers.PagedResult;
using Server.Domain.Entity.Content;

namespace Server.Application.Common.Interfaces.Persistence
{
    public interface IContributionRepository : IRepository<Contribution,Guid>
    {
        Task<PagedResult<ContributionInListDto>> GetAllPaging(string? keyword, Guid? yearId, Guid? facultyId, int pageIndex = 1, int pageSize = 10);
        bool IsConfirmed(Guid contributionId);
        Task<ContributionDto> GetContributionBySlug(string slug);

        Task<List<TagDto>> GetAllTags(Guid contributionId);
        
        Task<bool> IsSlugAlreadyExisted(string slug, Guid? id = null);
        Task Approve(Contribution contribution, Guid userId);
        Task SendToApprove(Guid contributionId,Guid userId);
        Task Reject(Contribution contribution,Guid userId, string note);
        Task<string> GetRejectReason(Contribution contribution);
        Task<List<ActivityLogDto>> GetActivityLogs(Contribution contribution);
    }
}
