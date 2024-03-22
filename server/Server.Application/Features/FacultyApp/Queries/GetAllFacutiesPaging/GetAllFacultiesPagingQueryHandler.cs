using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Faculties;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;
using Server.Application.Wrappers.PagedResult;

namespace Server.Application.Features.FacultyApp.Queries.GetAllFacutiesPaging;

public class GetAllFacultiesPagingQueryHandler
    : IRequestHandler<GetAllFacultiesPagingQuery, ErrorOr<IResponseWrapper<PagedResult<FacultyDto>>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllFacultiesPagingQueryHandler(IUnitOfWork unitOfWork)
    {        
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<IResponseWrapper<PagedResult<FacultyDto>>>> Handle(
        GetAllFacultiesPagingQuery request, 
        CancellationToken cancellationToken)
    {   

        var responseData =
             await _unitOfWork
                .FacultyRepository
                .GetAllFacultiesPaging(request.Keyword, request.PageIndex, request.PageSize);

        return new ResponseWrapper<PagedResult<FacultyDto>>
        {
            IsSuccessfull = true,
            Messages = new List<string> { $"Get All paging Faculties successfully!)" },
            ResponseData = responseData,
        };
    }
}