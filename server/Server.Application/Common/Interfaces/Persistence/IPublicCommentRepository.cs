using Server.Application.Common.Dtos.Comments;
using Server.Domain.Entity.Content;

namespace Server.Application.Common.Interfaces.Persistence
{
    public interface IPublicCommentRepository : IRepository<ContributionPublicComment, Guid>
    {
        Task<List<CommentDto>> GetCommentByContribution(Guid contributionId);
    }
}
