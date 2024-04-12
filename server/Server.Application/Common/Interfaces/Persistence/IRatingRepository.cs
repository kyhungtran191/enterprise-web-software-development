using Server.Domain.Entity.Content;

namespace Server.Application.Common.Interfaces.Persistence
{
    public interface IRatingRepository : IRepository<ContributionPublicRating,Guid>
    {
        Task<double> GetAverageRatingAsync(Guid contributionPublicId);
    }
}
