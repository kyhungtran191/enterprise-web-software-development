using ErrorOr;
using MediatR;
using Server.Application.Wrappers;

namespace Server.Application.Features.TagApp.Commands.DeleteTag
{
    public class DeleteTagCommand : IRequest<ErrorOr<IResponseWrapper>>
    {
        public List<Guid> TagIds { get; set; }
    }
}
