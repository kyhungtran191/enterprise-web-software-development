using System.Runtime.CompilerServices;
using ErrorOr;
using MediatR;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Common.Interfaces.Services;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;

namespace Server.Application.Features.ContributionApp.Commands.DeleteContribution
{
    public class DeleteContributionCommandHandler : IRequestHandler<DeleteContributionCommand, ErrorOr<IResponseWrapper>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;

        public DeleteContributionCommandHandler(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
            _unitOfWork = unitOfWork;
        }

        public async  Task<ErrorOr<IResponseWrapper>> Handle(DeleteContributionCommand request, CancellationToken cancellationToken)
        {
            var successfullyDeletedItems = new List<Guid>();

            foreach (var id in request.ContributionIds)
            {
                var item = await _unitOfWork.ContributionRepository.GetByIdAsync(id);
                if (item == null)
                {
                    return Errors.Contribution.NotFound;
                }

                item.DateDeleted = _dateTimeProvider.UtcNow;
                successfullyDeletedItems.Add(id);
            }

            await _unitOfWork.CompleteAsync();
            return new ResponseWrapper
            {
                IsSuccessfull = true,
                Messages = new List<string>
                {
                    $"Successfully deleted {successfullyDeletedItems.Count} items.",
                    "Each item is available for recovery."
                },
            };

        }
    }
}
