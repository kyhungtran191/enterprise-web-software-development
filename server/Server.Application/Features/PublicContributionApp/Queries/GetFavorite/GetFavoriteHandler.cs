using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;

namespace Server.Application.Features.PublicContributionApp.Queries.GetFavorite
{
    public class GetFavoriteHandler : IRequestHandler<GetFavoriteQuery, ErrorOr<IResponseWrapper<List<PublicContributionInListDto>>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetFavoriteHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<IResponseWrapper<List<PublicContributionInListDto>>>> Handle(GetFavoriteQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.PublicContributionRepository.GetUserFavoriteContributions(request.UserId);
            return new ResponseWrapper<List<PublicContributionInListDto>>
            {
                IsSuccessfull = true,
                Messages = new List<string> { $"Get favorite list successfully" },
                ResponseData = result,
            };
        }
    }
}
