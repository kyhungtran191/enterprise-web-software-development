using ErrorOr;
using MediatR;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Common.Interfaces.Services;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;

namespace Server.Application.Features.AcademicYearApp.Commands.DeleteAcademicYear
{  public class DeleteAcademicYearCommandHandler : IRequestHandler<DeleteAcademicYearCommand,ErrorOr<IResponseWrapper>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;

        public DeleteAcademicYearCommandHandler(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<IResponseWrapper>> Handle(DeleteAcademicYearCommand request, CancellationToken cancellationToken)
        {
            var errors = new List<Error>();
            var successfullyDeletedItems = new List<Guid>();
            foreach (var id in request.YearIds)
            {
                var yearFromDb = await _unitOfWork.AcademicYearRepository.GetByIdAsync(id);
                if (yearFromDb == null)
                {
                    errors.Add(Errors.AcademicYear.NotFound);
                }
                // check contribution still in this academic year (later)
                if (errors.Count == 0)
                {
                    yearFromDb.DateDeleted = _dateTimeProvider.UtcNow;
                    successfullyDeletedItems.Add(id);
                }
            }

            await _unitOfWork.CompleteAsync();
            if (successfullyDeletedItems.Count > 0)
            {
                return new ResponseWrapper
                {
                    IsSuccessfull = true,
                    Messages = new List<string>
                    {
                        $"Successfully deleted {successfullyDeletedItems.Count} academic years.",
                        "Each item is available for recovery."
                    }
                };
            }

            return errors;
        }
    }
}
