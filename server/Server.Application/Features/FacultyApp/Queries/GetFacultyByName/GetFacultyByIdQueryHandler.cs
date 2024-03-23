using AutoMapper;
using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Faculties;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Content;

namespace Server.Application.Features.FacultyApp.Queries.GetFacultyByName;

public class GetFacultyByIdQueryHandler
    : IRequestHandler<GetFacultyByIdQuery, ErrorOr<IResponseWrapper<FacultyDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetFacultyByIdQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<IResponseWrapper<FacultyDto>>> Handle(
        GetFacultyByIdQuery request,
        CancellationToken cancellationToken)
    {
        if (await _unitOfWork.FacultyRepository.GetByIdAsync(request.FacultyId) is not Faculty facultyFromDb)
        {
            return Errors.Faculty.CannotFound;
        }

        return new ResponseWrapper<FacultyDto>
        {
            IsSuccessfull = true,
            Messages = new List<string> { $"Get Faculty ({facultyFromDb.Name})" },
            ResponseData = _mapper.Map<FacultyDto>(facultyFromDb),
        };
    }
}