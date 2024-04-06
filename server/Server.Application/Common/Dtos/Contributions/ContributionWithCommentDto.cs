using Server.Application.Common.Dtos.Comments;

namespace Server.Application.Common.Dtos.Contributions
{
    public class ContributionWithCommentDto : ContributionDto
    {
        public List<CommentDto> Comments { get; set; }
    }
}
