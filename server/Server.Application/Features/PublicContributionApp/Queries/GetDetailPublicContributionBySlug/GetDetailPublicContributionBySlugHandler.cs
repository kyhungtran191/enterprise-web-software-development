using System.Collections.Generic;
using AutoMapper;
using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;

namespace Server.Application.Features.PublicContributionApp.Queries.GetDetailPublicContributionBySlug
{
    public class GetDetailPublicContributionBySlugHandler : IRequestHandler<GetDetailPublicContributionBySlugQuery, ErrorOr<IResponseWrapper<PublicContributionWithCommentDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDetailPublicContributionBySlugHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ErrorOr<IResponseWrapper<PublicContributionWithCommentDto>>> Handle(GetDetailPublicContributionBySlugQuery request, CancellationToken cancellationToken)
        {
            var itemFromDb = await _unitOfWork.PublicContributionRepository.GetBySlug(request.Slug);
            if (itemFromDb == null)
            {
                return Errors.Contribution.NotFoundPublic;
            }
            itemFromDb.View += 1;
            var comments = await _unitOfWork.PublicCommentRepository.GetCommentByContribution(itemFromDb.Id);
            var result = _mapper.Map<PublicContributionWithCommentDto>(itemFromDb);
            result.Comments = comments;
            await _unitOfWork.CompleteAsync();
            return new ResponseWrapper<PublicContributionWithCommentDto>
            {
                IsSuccessfull = true,
                Messages = new List<string> { $"Get Contribution By Slug Success" },
                ResponseData = result
            };
        }
    }
}
