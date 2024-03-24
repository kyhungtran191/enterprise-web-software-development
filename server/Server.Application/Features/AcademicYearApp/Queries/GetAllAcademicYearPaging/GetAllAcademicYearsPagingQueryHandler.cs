using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.AcademicYears;
using Server.Application.Wrappers.PagedResult;
using Server.Application.Wrappers;
using Server.Application.Common.Interfaces.Persistence;

namespace Server.Application.Features.AcademicYearApp.Queries.GetAllAcademicYearPaging
{
    public class GetAllAcademicYearsPagingQueryHandler : IRequestHandler<GetAllAcademicYearsPagingQuery, ErrorOr<IResponseWrapper<PagedResult<AcademicYearDto>>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllAcademicYearsPagingQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<IResponseWrapper<PagedResult<AcademicYearDto>>>> Handle(GetAllAcademicYearsPagingQuery request, CancellationToken cancellationToken)
        {
            var years = await _unitOfWork.AcademicYearRepository.GetAllYearsPaging(request.Keyword, request.PageIndex, request.PageSize);
            return new ResponseWrapper<PagedResult<AcademicYearDto>>
            {
                IsSuccessfull = true,
                Messages = new List<string>
                {
                    $"Get all paging academic years success"
                },
                ResponseData = years
            };
        }
    }
}
