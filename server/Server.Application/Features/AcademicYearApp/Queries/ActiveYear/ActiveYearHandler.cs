using ErrorOr;
using MediatR;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;

namespace Server.Application.Features.AcademicYearApp.Queries.ActiveYear
{
   
    public class ActiveYearHandler : IRequestHandler<ActiveYearCommand, ErrorOr<IResponseWrapper>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public ActiveYearHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<IResponseWrapper>> Handle(ActiveYearCommand request, CancellationToken cancellationToken)
        {
            if(await _unitOfWork.AcademicYearRepository.AnyActiveYear())
            {
                return Errors.AcademicYear.HasActiveYearAlready;
            }
            var item = await _unitOfWork.AcademicYearRepository.GetByIdAsync(request.YearId);
            if(item is null)
            {
                return Errors.AcademicYear.NotFound;
            }
            item.IsActive = true;
            await _unitOfWork.CompleteAsync();
            return new ResponseWrapper
            {
                IsSuccessfull = true,
                Messages = new List<string> { $"Active success" }
            };
        }
    }
}
