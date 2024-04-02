using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;

namespace Server.Application.Features.PublicContributionApp.Queries.GetLikedContribution
{
    public class GetLikeContributionHandler : IRequestHandler<GetLikedContributionQuery, ErrorOr<IResponseWrapper<List<PublicContributionInListDto>>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetLikeContributionHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<IResponseWrapper<List<PublicContributionInListDto>>>> Handle(GetLikedContributionQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.PublicContributionRepository.GetLikedContribution(request.UserId);
            return new ResponseWrapper<List<PublicContributionInListDto>>
            {
                IsSuccessfull = true,
                Messages = new List<string> { $"Your liked contributions list " },
                ResponseData = result
            };
        }
    }
}
