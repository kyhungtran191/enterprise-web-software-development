using Server.Application.Common.Dtos.Contributions;
using Server.Application.Wrappers.PagedResult;
using Server.Domain.Entity.Content;

namespace Server.Application.Common.Interfaces.Persistence
{
    public interface IPublicContributionRepository : IRepository<ContributionPublic,Guid>
    {
        Task<PagedResult<PublicContributionInListDto>> GetAllPaging(string? keyword, string? year, string? facultyName, string? status, int pageIndex = 1, int pageSize = 10);
        Task<List<PublicContributionInListDto>> GetTopContributions(int quantity = 4);
    }
}
