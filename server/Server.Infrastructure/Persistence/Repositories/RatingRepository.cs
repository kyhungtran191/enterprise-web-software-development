using Microsoft.EntityFrameworkCore;
using Server.Application.Common.Interfaces.Persistence;
using Server.Domain.Entity.Content;

namespace Server.Infrastructure.Persistence.Repositories
{
    public class RatingRepository : RepositoryBase<ContributionPublicRating,Guid> , IRatingRepository
    {
        private readonly AppDbContext _context;
        public RatingRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<double> GetAverageRatingAsync(Guid contributionPublicId)
        {
            var ratings = await _context.ContributionPublicRatings
                .Where(c => c.ContributionPublicId == contributionPublicId)
                .ToListAsync();

            return ratings.Count == 0 ? 0.0 : ratings.Average(r => r.Rating);
        }
    }
}
