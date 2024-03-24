using AutoMapper;
using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.AcademicYears;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Content;

namespace Server.Application.Features.AcademicYearApp.Queries.GetAcademicYearById
{
    public class GetAcademicYearByIdQueryHandler : IRequestHandler<GetAcademicYearByIdQuery, ErrorOr<IResponseWrapper<AcademicYearDto>>>

    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAcademicYearByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ErrorOr<IResponseWrapper<AcademicYearDto>>> Handle(GetAcademicYearByIdQuery request, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.AcademicYearRepository.GetByIdAsync(request.YearId) is not AcademicYear yearFromDb)
            {
                return Errors.AcademicYear.NotFound;
            }

            return new ResponseWrapper<AcademicYearDto>
            {
                IsSuccessfull = true,
                Messages = new List<string> { $"Get Academic Year '{yearFromDb.Name}' success" },
                ResponseData = _mapper.Map<AcademicYearDto>(yearFromDb)
            };
        }
    }
}
