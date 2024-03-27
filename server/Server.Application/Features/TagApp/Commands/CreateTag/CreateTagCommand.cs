using ErrorOr;
using MediatR;
using Server.Application.Wrappers;

namespace Server.Application.Features.TagApp.Commands.CreateTag
{
    public class CreateTagCommand : IRequest<ErrorOr<IResponseWrapper>>
    {
        public string TagName { get; set; } = null!;
    }
}
