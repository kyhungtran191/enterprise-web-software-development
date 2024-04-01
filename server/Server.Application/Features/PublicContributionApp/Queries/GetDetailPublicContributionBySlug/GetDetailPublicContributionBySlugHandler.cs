using System.Collections.Generic;
using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;

namespace Server.Application.Features.PublicContributionApp.Queries.GetDetailPublicContributionBySlug
{
    public class GetDetailPublicContributionBySlugHandler : IRequestHandler<GetDetailPublicContributionBySlugQuery, ErrorOr<IResponseWrapper<PublicContributionDetailDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetDetailPublicContributionBySlugHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<IResponseWrapper<PublicContributionDetailDto>>> Handle(GetDetailPublicContributionBySlugQuery request, CancellationToken cancellationToken)
        {
            var itemFromDb = await _unitOfWork.PublicContributionRepository.GetBySlug(request.Slug);
            if (itemFromDb == null)
            {
                return Errors.Contribution.NotFoundPublic;
            }
            itemFromDb.View += 1;
            await _unitOfWork.CompleteAsync();
            return new ResponseWrapper<PublicContributionDetailDto>
            {
                IsSuccessfull = true,
                Messages = new List<string> { $"Get Contribution By Slug Success" },
                ResponseData = itemFromDb
            };
        }
    }
}
