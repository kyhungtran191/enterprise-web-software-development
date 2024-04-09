using ErrorOr;
using MediatR;
using Server.Application.Wrappers;

namespace Server.Application.Features.PublicCommentApp.Commands.CreateComment
{
    public class CreatePublicCommentCommand : IRequest<ErrorOr<IResponseWrapper>>
    {
        public Guid ContributionId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
    }
}
