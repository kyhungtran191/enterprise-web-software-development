using Microsoft.EntityFrameworkCore;
using Server.Application.Common.Dtos.Comments;
using Server.Application.Common.Interfaces.Persistence;
using Server.Domain.Entity.Content;

namespace Server.Infrastructure.Persistence.Repositories
{
    public class PublicCommentRepository : RepositoryBase<ContributionPublicComment, Guid>, IPublicCommentRepository
    {
        private readonly AppDbContext _appDbContext;
        public PublicCommentRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<List<CommentDto>> GetCommentByContribution(Guid contributionId)
        {
            var commentsWithUserDetails = await _appDbContext.ContributionPublicComments
                .Where(x => x.ContributionId == contributionId)
                .OrderBy(comment => comment.DateCreated)
                .Select(comment => new
                {
                    comment.Content,
                    comment.DateCreated,
                    User = _appDbContext.Users.Where(x => x.Id == comment.UserId).FirstOrDefault()
                })
                .ToListAsync();
            var commentDtos = commentsWithUserDetails.Select(c => new CommentDto
            {
                Content = c.Content,
                DateCreated = c.DateCreated,
                UserName = c.User.UserName,
                Avatar = c.User.Avatar
            }).ToList();
            return commentDtos;
        }
    }
}
