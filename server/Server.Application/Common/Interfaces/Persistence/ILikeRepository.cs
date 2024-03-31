using Server.Domain.Entity.Content;

namespace Server.Application.Common.Interfaces.Persistence
{
    public interface ILikeRepository : IRepository<Like,Guid>
    {
        Task<int> GetNumberLikeOfContribution(ContributionPublic contribution);
        Task<bool> AlreadyLike(ContributionPublic contribution, Guid userId);
        Task<Like> GetSpecificLike(ContributionPublic contribution, Guid userId);
    }
}
