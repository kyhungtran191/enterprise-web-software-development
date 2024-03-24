using ErrorOr;
using MediatR;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Common.Interfaces.Services;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Content;

namespace Server.Application.Features.AcademicYearApp.Commands.UpdateAcademicYear
{
    public class UpdateAcademicYearCommandHandler : IRequestHandler<UpdateAcademicYearCommand, ErrorOr<IResponseWrapper>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;


        public UpdateAcademicYearCommandHandler(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;

        }
        public async Task<ErrorOr<IResponseWrapper>> Handle(UpdateAcademicYearCommand request, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.AcademicYearRepository.GetByIdAsync(request.AcademicYearId) is not AcademicYear yearFromDb)
            {
                return Errors.AcademicYear.NotFound;
            }

            if (yearFromDb.DateDeleted.HasValue)
            {
                return Errors.AcademicYear.Deleted;
            }

            if (await _unitOfWork.AcademicYearRepository.GetAcademicYearByName(request.AcademicYearName) is not null)
            {
                return Errors.AcademicYear.AlreadyExist;
            }
            yearFromDb.Name = request.AcademicYearName;
            yearFromDb.FinalClosureDate = request.FinalClosureDate;
            yearFromDb.EndClosureDate = request.EndClosureDate;
            yearFromDb.StartClosureDate = request.StartClosureDate;
            yearFromDb.DateEdited = _dateTimeProvider.UtcNow;
            await _unitOfWork.CompleteAsync();
            return new ResponseWrapper
            {
                IsSuccessfull = true,
                Messages = new List<string>
                {
                    "Update Academic Year successfully"
                }
            };

        }
    }
}
