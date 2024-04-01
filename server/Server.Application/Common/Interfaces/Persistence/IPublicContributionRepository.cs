using Server.Application.Common.Dtos.Contributions;
using Server.Application.Wrappers.PagedResult;
using Server.Domain.Entity.Content;
using static Server.Domain.Common.Errors.Errors;

namespace Server.Application.Common.Interfaces.Persistence
{
    public interface IPublicContributionRepository : IRepository<ContributionPublic,Guid>
    {
        Task<PagedResult<PublicContributionInListDto>> GetAllPaging(string? keyword, string? year, string? facultyName, string? status, int pageIndex = 1, int pageSize = 10);
        Task<List<PublicContributionInListDto>> GetFeaturedContribution();
        Task<List<TopContributorDto>> GetTopContributors();
        Task<PublicContributionDetailDto> GetBySlug(string slug);
        Task<List<PublicContributionInListDto>> GetTopContributions(int quantity = 4);
        Task AddToReadLater(ContributionPublic contribution, Guid userId);
        Task<bool> AlreadyReadLater(ContributionPublic contribution, Guid userId);
        Task<ContributionPublicReadLater> GetReadLater(ContributionPublic contribution, Guid userId);
        void RemoveReadLater(ContributionPublicReadLater readLater);
        Task AddToFavorite(ContributionPublic contribution, Guid userId);
        Task<bool> AlreadyFavorite(ContributionPublic contribution,Guid userId);
        Task<ContributionPublicFavorite> GetFavorite(ContributionPublic contribution, Guid userId);
        void RemoveFavorite(ContributionPublicFavorite favorite);
        Task<List<PublicContributionInListDto>> GetUserReadLaterContributions(Guid userId);
        Task<List<PublicContributionInListDto>> GetUserFavoriteContributions(Guid userId);
    }
}
