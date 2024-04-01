
using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Server.Application.Common.Dtos.Users;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Identity;

namespace Server.Application.Features.Identity.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler
    : IRequestHandler<GetUserByIdQuery, ErrorOr<IResponseWrapper<UserDto>>>
{
    private readonly IFacultyRepository _facultyRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(UserManager<AppUser> userManager, IMapper mapper, IFacultyRepository facultyRepository)
    {
        _userManager = userManager;
        _mapper = mapper;
        _facultyRepository = facultyRepository;
    }

    public async Task<ErrorOr<IResponseWrapper<UserDto>>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var userFromDb = await _userManager.FindByIdAsync(request.Id.ToString());

        if (userFromDb is null)
        {
            return Errors.User.CannotFound;
        }

        if (userFromDb.IsActive == false)
        {
            return Errors.User.InactiveOrEnableLocked;
        }

        var result = _mapper.Map<UserDto>(userFromDb);

        if (userFromDb.FacultyId is not null)
        {
            var faculty = await _facultyRepository.GetByIdAsync(userFromDb.FacultyId.Value);
            result.Faculty = faculty.Name ?? null;
        }
        
        var roles = await _userManager.GetRolesAsync(userFromDb);
        result.Roles = roles;

        return new ResponseWrapper<UserDto>
        {
            IsSuccessfull = true,
            Messages = new List<string> { "Get user by id successfully!" },
            ResponseData = result,
        };
    }
}

