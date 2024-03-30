using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Server.Application.Common.Interfaces.Persistence;
using Server.Domain.Entity.Content;

namespace Server.Infrastructure.Persistence.Repositories
{
    public class LikeRepository : RepositoryBase<Like,Guid> , ILikeRepository
    {
        private readonly AppDbContext _appDbContext;

        public LikeRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> AlreadyLike(ContributionPublic contribution, Guid userId)
        {
            return await _appDbContext.Likes
                .AnyAsync(l => l.ContributionPublicId == contribution.Id && l.UserId == userId);
        }

        public async Task<int> GetNumberLikeOfContribution(ContributionPublic contribution)
        {
            var count = await _appDbContext.Likes.CountAsync(l => l.ContributionPublicId == contribution.Id);
            return count;
        }

        public async Task<Like> GetSpecificLike(ContributionPublic contribution, Guid userId)
        {
            var like = await _appDbContext.Likes.Where(l =>
                l.ContributionPublicId == contribution.Id && l.UserId == userId).FirstOrDefaultAsync();
            return like;
        }
    }
}
