using ErrorOr;
using MediatR;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Common.Interfaces.Services;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;

namespace Server.Application.Features.TagApp.Commands.DeleteTag
{
    public class DeleteTagCommandHandler : IRequestHandler<DeleteTagCommand, ErrorOr<IResponseWrapper>>

    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;
        // inject TagContributionRepo later
        public DeleteTagCommandHandler(IUnitOfWork unitOfWork,IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<IResponseWrapper>> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            var errors = new List<Error>();
            var successfullyDeletedTags = new List<Guid>();
            foreach(var tagId in request.TagIds)
            {
                var tagFromDb = await _unitOfWork.TagRepository.GetByIdAsync(tagId);
                if(tagFromDb == null)
                {
                    errors.Add(Errors.Tags.CannotFound);
             
                }
                // check tag in tagcontributionrepo later
                tagFromDb.DateDeleted = _dateTimeProvider.UtcNow;
                successfullyDeletedTags.Add(tagId);
            }
            await _unitOfWork.CompleteAsync();
            if(successfullyDeletedTags.Count > 0)
            {
                return new ResponseWrapper
                {
                    IsSuccessfull = true,
                    Messages = new List<string>
                    {
                        $"Successfully deleted {successfullyDeletedTags.Count} faculties.",
                        "Each faculty is available for recovery."
                    },
                };
            }
            return errors;
        }
    }
}
