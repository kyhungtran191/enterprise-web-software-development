using System.Collections.Generic;
using ErrorOr;
using MediatR;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;

namespace Server.Application.Features.PublicContributionApp.Commands.ViewContribution
{
    public class ViewContributionHandler : IRequestHandler<ViewContributionCommand, ErrorOr<IResponseWrapper>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public ViewContributionHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<IResponseWrapper>> Handle(ViewContributionCommand request, CancellationToken cancellationToken)
        {
            var contribution = await _unitOfWork.PublicContributionRepository.GetByIdAsync(request.ContributionId);
            if (contribution is null)
            {
                return Errors.Contribution.NotFound;
            }
            contribution.Views += 1;
            await _unitOfWork.CompleteAsync();
            return new ResponseWrapper
            {
                IsSuccessfull = true,
                Messages = new List<string>
                {
                    $"Plus one View"
                }
            };

        }
    }
}
