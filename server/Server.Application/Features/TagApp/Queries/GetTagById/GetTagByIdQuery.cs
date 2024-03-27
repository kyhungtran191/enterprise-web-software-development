using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Tags;
using Server.Application.Wrappers;

namespace Server.Application.Features.TagApp.Queries.GetTagById
{
    public class GetTagByIdQuery : IRequest<ErrorOr<IResponseWrapper<TagDto>>>
    {
        public Guid TagId { get; set; }
    }
}
