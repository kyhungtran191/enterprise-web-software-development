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
using Server.Application.Wrappers.PagedResult;

namespace Server.Application.Features.ContributionApp.Queries.GetNotCommentContribution
{
    public class GetNotCommentContributionHandler : IRequestHandler<GetNotCommentContributionQuery, ErrorOr<IResponseWrapper<PagedResult<NotCommentContributionDto>>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetNotCommentContributionHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<IResponseWrapper<PagedResult<NotCommentContributionDto>>>> Handle(GetNotCommentContributionQuery request, CancellationToken cancellationToken)
        {
            var items = await _unitOfWork.ContributionRepository.GetUncommentedContributions(request.PageIndex,request.PageSize);
            return new ResponseWrapper<PagedResult<NotCommentContributionDto>>
            {
                IsSuccessfull = true,
                Messages = new List<string> { $"Get list un comment success" },
                ResponseData = items
            };
        }
    }
}
