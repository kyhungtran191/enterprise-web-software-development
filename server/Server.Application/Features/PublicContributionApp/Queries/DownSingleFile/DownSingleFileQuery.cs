using ErrorOr;
using MediatR;
using Server.Application.Wrappers;

namespace Server.Application.Features.PublicContributionApp.Queries.DownSingleFile
{
    public class DownSingleFileQuery :  IRequest<ErrorOr<IResponseWrapper<string>>>
    {
        public List<string> PublicIds { get; set; }
    }
}
