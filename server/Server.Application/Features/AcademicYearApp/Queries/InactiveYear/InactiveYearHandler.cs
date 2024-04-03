using ErrorOr;
using MediatR;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Features.AcademicYearApp.Queries.ActiveYear;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Server.Application.Features.AcademicYearApp.Queries.InactiveYear
{
    public class InactiveYearHandler : IRequestHandler<InactiveYearCommand, ErrorOr<IResponseWrapper>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public InactiveYearHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<IResponseWrapper>> Handle(InactiveYearCommand request, CancellationToken cancellationToken)
        {
            var item = await _unitOfWork.AcademicYearRepository.GetByIdAsync(request.YearId);
            if (item is null)
            {
                return Errors.AcademicYear.NotFound;
            }

            if (!item.IsActive)
            {
                return Errors.AcademicYear.AlreadyInactive;
            }
            item.IsActive = false;
            await _unitOfWork.CompleteAsync();
            return new ResponseWrapper
            {
                IsSuccessfull = true,
                Messages = new List<string> { $"Inactive success" }
            }; ;
        }
    }
}
