using ErrorOr;
using MediatR;
using Server.Application.Wrappers;

namespace Server.Application.Features.CommentApp.Commands
{
    public class CreateCommentCommand : IRequest<ErrorOr<IResponseWrapper>>
    {
        public Guid ContributionId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
    }
}
