using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Users;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;

namespace Server.Application.Features.PublicContributionApp.Queries.GetListUserLiked
{
    public class GetListUserLikedHandler : IRequestHandler<GetListUserLikedQuery, ErrorOr<IResponseWrapper<List<UserInListDto>>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetListUserLikedHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<IResponseWrapper<List<UserInListDto>>>> Handle(GetListUserLikedQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.PublicContributionRepository.GetListUserLiked(request.ContributionId);
            return new ResponseWrapper<List<UserInListDto>>
            {
                IsSuccessfull = true,
                Messages = new List<string> { $"Get list user liked success" },
                ResponseData = result
            };
        }
    }
}
