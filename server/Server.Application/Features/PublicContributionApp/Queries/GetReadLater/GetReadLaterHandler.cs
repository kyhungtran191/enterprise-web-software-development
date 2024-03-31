using System.Collections.Generic;
using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;

namespace Server.Application.Features.PublicContributionApp.Queries.GetReadLater
{
    public class GetReadLaterHandler : IRequestHandler<GetReadLaterQuery, ErrorOr<IResponseWrapper<List<PublicContributionInListDto>>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetReadLaterHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<IResponseWrapper<List<PublicContributionInListDto>>>> Handle(GetReadLaterQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.PublicContributionRepository.GetUserReadLaterContributions(request.UserId);
            return new ResponseWrapper<List<PublicContributionInListDto>>
            {
                IsSuccessfull = true,
                Messages = new List<string> { $"Get list read later successfully" },
                ResponseData = result,
            };
        }
    }
}
