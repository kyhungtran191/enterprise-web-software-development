using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server.Application.Common.Dtos.Users;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;
using Server.Application.Wrappers.PagedResult;
using Server.Domain.Entity.Identity;

namespace Server.Application.Features.Identity.Users.Queries.GetAllUsersPaging;

//  public string? Keyword { get; set; }
//     public int PageIndex { get; set; } = 1;
//     public int PageSize { get; set; } = 10;
public class GetAllUserPagingQueryHandler
    : IRequestHandler<GetAllUserPagingQuery, ErrorOr<IResponseWrapper<PagedResult<UserDto>>>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IFacultyRepository _facultyRepository;
    private readonly IMapper _mapper;

    public GetAllUserPagingQueryHandler(UserManager<AppUser> userManager, IMapper mapper, IFacultyRepository facultyRepository)
    {
        _userManager = userManager;
        _mapper = mapper;
        _facultyRepository = facultyRepository;
    }

    public async Task<ErrorOr<IResponseWrapper<PagedResult<UserDto>>>> Handle(GetAllUserPagingQuery request, CancellationToken cancellationToken)
    {
        var query = _userManager.Users;

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            query = query.Where(
                user => user.UserName!.Contains(request.Keyword)
                        || user.Email!.Contains(request.Keyword)                        
                        || user.FirstName.Contains(request.Keyword)
                        || user.LastName.Contains(request.Keyword)
                        || user.PhoneNumber!.Contains(request.Keyword)
            );
        }

        var count = await query.CountAsync(cancellationToken);

        var skipPage = (request.PageIndex - 1) * request.PageSize;

        query =
             query
            .Skip(skipPage)
            .Take(request.PageSize)
            .OrderByDescending(x => x.DateCreated);

        var result = await _mapper.ProjectTo<UserDto>(query).ToListAsync(cancellationToken);

        // get role        
        foreach (var userDto in result)
        {
            var userFromDb = await _userManager.FindByIdAsync(userDto.Id.ToString());            

            if (userFromDb?.FacultyId is not null)
            {
                var facultyFromDb = await _facultyRepository.GetByIdAsync(userFromDb.FacultyId.Value);
                userDto.Faculty = facultyFromDb.Name ?? null;

            }
            var roles = await _userManager.GetRolesAsync(userFromDb!);

            userDto.Roles = roles;
        }

        return new ResponseWrapper<PagedResult<UserDto>>
        {
            IsSuccessfull = true,
            ResponseData = new PagedResult<UserDto>
            {
                CurrentPage = request.PageIndex,                
                PageSize = request.PageSize,
                Results = result,
                RowCount = count,
            }
        };
    }
}